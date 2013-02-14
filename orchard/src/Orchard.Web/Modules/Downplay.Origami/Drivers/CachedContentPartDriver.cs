using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Caching;
using Orchard.DisplayManagement.Shapes;

namespace Downplay.Origami.Drivers {
    public abstract class CachedContentPartDriver<TContent,TCacheKey> : ContentPartDriver<TContent> where TContent : ContentPart, new() {

        private readonly ICacheManager _cacheManager;

        public CachedContentPartDriver(ICacheManager cacheManager) {
            _cacheManager = cacheManager;
        }

        public CachedContentShapeResult<TCacheKey> Cached(string shapeType, Func<TCacheKey> cacheKey, Func<dynamic> factory) {
            return new CachedContentShapeResult<TCacheKey>(shapeType, Prefix, (context) => factory(), cacheKey, (key) => _cacheManager.Get<TCacheKey, CachedShape>(key, (ctx) => {
                // TODO: Support monitors
                return new CachedShape();
            }));
        }

    }
    public class CachedShape {
        public string Html { get; set; }
    }

    /// <summary>
    /// TODO: Review. Does this work? Do we have a better method that doesn't require special driver/result? (Unlikely). Inherit from a base result to avoid all the duplication (not to
    /// mention problematic differentiator/etc.)
    /// </summary>
    /// <typeparam name="TCacheKey"></typeparam>
    public class CachedContentShapeResult<TCacheKey> : ContentShapeResult {
        private string _defaultLocation;
        private string _differentiator;
        private readonly string _shapeType;
        private readonly string _prefix;
        private readonly Func<BuildShapeContext, dynamic> _shapeBuilder;
        private string _groupId;

        private readonly Func<TCacheKey> _keyBuilder;
        private readonly Func<TCacheKey, dynamic> _cacheAccessor;

        public CachedContentShapeResult(string shapeType, string prefix, Func<BuildShapeContext, dynamic> shapeBuilder, Func<TCacheKey> keyBuilder, Func<TCacheKey, CachedShape> cacheAccessor)
            : base(shapeType, prefix, shapeBuilder) {
            _shapeType = shapeType;
            _prefix = prefix;
            _shapeBuilder = shapeBuilder;
            _keyBuilder = keyBuilder;
                _cacheAccessor = cacheAccessor;
        }

        public override void Apply(BuildDisplayContext context) {
            ApplyImplementation(context, context.DisplayType);
        }

        public override void Apply(BuildEditorContext context) {
            ApplyImplementation(context, null);
        }

        private void ApplyImplementation(BuildShapeContext context, string displayType) {
            if (!string.Equals(context.GroupId ?? "", _groupId ?? "", StringComparison.OrdinalIgnoreCase))
                return;

            var placement = context.FindPlacement(_shapeType, _differentiator, _defaultLocation);
            if (string.IsNullOrEmpty(placement.Location) || placement.Location == "-")
                return;

            dynamic parentShape = context.Shape;
            dynamic newShape;
            var key = _keyBuilder();
            var cache = _cacheAccessor(key);

            if (cache.Html != null) {
                newShape = context.New.CachedShape(ChildContent: cache.Html);
            }
            else {
                newShape = _shapeBuilder(context);
                newShape.CacheMetadata = cache;
            }
            
            // Only need to bother with metadata if result isn't cached
            // TODO: Maybe some will end up useful anyway, e.g. placement source, displaytype
            if (cache.Html==null) {
                ShapeMetadata newShapeMetadata = newShape.Metadata;
                newShapeMetadata.Prefix = _prefix;
                newShapeMetadata.DisplayType = displayType;
                newShapeMetadata.PlacementSource = placement.Source;

                // if a specific shape is provided, remove all previous alternates and wrappers
                if (!String.IsNullOrEmpty(placement.ShapeType)) {
                    newShapeMetadata.Type = placement.ShapeType;
                    newShapeMetadata.Alternates.Clear();
                    newShapeMetadata.Wrappers.Clear();
                }

                foreach (var alternate in placement.Alternates) {
                    newShapeMetadata.Alternates.Add(alternate);
                }

                foreach (var wrapper in placement.Wrappers) {
                    newShapeMetadata.Wrappers.Add(wrapper);
                }
            }
            
            var delimiterIndex = placement.Location.IndexOf(':');
            if (delimiterIndex < 0) {
                parentShape.Zones[placement.Location].Add(newShape);
            }
            else {
                var zoneName = placement.Location.Substring(0, delimiterIndex);
                var position = placement.Location.Substring(delimiterIndex + 1);
                parentShape.Zones[zoneName].Add(newShape, position);
            }
        }



    }
}