using System;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement.Shapes;
using Downplay.Origami.Placement;

namespace Downplay.Origami.Services {

    public class ModelShapeResult : ModelShapeResultBase {
        private readonly Func<ModelShapeContext, dynamic> _shapeBuilder;
        public ModelShapeResult(string shapeType, string prefix, Func<ModelShapeContext, dynamic> shapeBuilder)
            : base(shapeType, prefix) {
            _shapeBuilder = shapeBuilder;
        }

        protected override dynamic BuildShape(ModelShapeContext context) {
            return _shapeBuilder(context);
        }
    }
    public abstract class ModelShapeResultBase : ModelDriverResult {
        private string _defaultLocation;
        private string _differentiator;
        private readonly string _shapeType;
        private readonly string _prefix;
        private string _groupId;

        public ModelShapeResultBase(string shapeType, string prefix) {
            _shapeType = shapeType;
            _prefix = prefix;
        }

        public override void Apply(ModelShapeContext context) {
            ApplyImplementation(context, context.DisplayType);
        }
        protected abstract dynamic BuildShape(ModelShapeContext context);

        private void ApplyImplementation(ModelShapeContext context, string displayType)
        {
            // Match groups
            // TODO: Technically this can be achieved thru placement matches now ...
            if (context.GroupId!=null) {
                if (!String.Equals(context.GroupId, _groupId ?? "", StringComparison.OrdinalIgnoreCase))
                    return;
            }

            var placement = context.FindPlacement(_shapeType, _differentiator, _defaultLocation);

            if (string.IsNullOrEmpty(placement.Location) || placement.Location == "-")
                return;

            dynamic parentShape = context.Shape;
            var newShape = BuildShape(context);
            // Handle null shape without an error
            if (newShape == null) return;

            ShapeMetadata newShapeMetadata = newShape.Metadata;
            newShapeMetadata.Prefix = _prefix;
            // Display type might have already been set (so we're not forced to use the one from the context)
            if (string.IsNullOrWhiteSpace(newShapeMetadata.DisplayType))
            {
                newShapeMetadata.DisplayType = displayType;
            }
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

            // TODO: One use of mutators is to add a new paradigm; might not be much use if the paradigm is added *after* placement has processed
            var modelPlacement = placement as ModelPlacementInfo;
            if (modelPlacement!=null) {
                foreach (var mutator in modelPlacement.Mutators)
                {
                    mutator.Invoke(modelPlacement, parentShape, newShape, newShapeMetadata, context);
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

        public ModelShapeResultBase Location(string zone) {
            _defaultLocation = zone;
            return this;
        }

        public ModelShapeResultBase Differentiator(string differentiator) {
            _differentiator = differentiator;
            return this;
        }

        public ModelShapeResultBase OnGroup(string groupId)
        {
            _groupId=groupId;
            return this;
        }
    }
}