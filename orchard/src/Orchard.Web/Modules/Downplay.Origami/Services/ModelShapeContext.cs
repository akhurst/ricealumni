using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;

namespace Downplay.Origami.Services
{
    /// <summary>
    /// </summary>
    public class ModelShapeContext
    {
        public ModelShapeContext(object model,IShapeFactory shapeFactory) {
            Mode = "Display";
            Model = model;
            New = shapeFactory;
            DisplayType = "Detail";
            // Initializers
            FindPlacement = (partType, differentiator, defaultLocation) => new PlacementInfo { Location = defaultLocation, Source = String.Empty };
            ChainedResults = new List<ModelChainContext>();
            Paradigms = new ParadigmsContext();
            CustomContext = new Dictionary<Type, object>();
   //         Stuff = new Stuff();
        }

        /// <summary>
        /// The Model itself
        /// </summary>
        public object Model { get; set; }
        
        /// <summary>
        /// The new way to say Display/Editor (can reuse for other distinctly different modes e.g. Email...)
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// The shape being built onto
        /// </summary>
        public dynamic Shape { get; set; }

        public FactoryDictionary<string, dynamic> DestinationShapes { get; set; }
        
        /// <summary>
        /// Name of type of display (Detail, Summary, etc.)
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Prefix for form generation
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Shape Factory
        /// </summary>
        public dynamic New { get; private set; }

        /// <summary>
        /// Delegate for binding a placement finder (slightly legacy in implementation)
        /// </summary>
        public Func<string, string, string, PlacementInfo> FindPlacement { get; set; }

        /// <summary>
        /// List of Paradigms in play
        /// TODO: Paradigms still only mean much in respect of sockets, not convinced they should be here at all
        /// </summary>
        public ParadigmsContext Paradigms { get; set; }

        /// <summary>
        /// Model binder for updates from forms, if available
        /// </summary>
        public IUpdateModel Updater { get; set; }

        /// <summary>
        /// Legacy from Content shapes
        /// TODO: Don't particularly need it here, it's bound directly to placement, and "Stereotype"
        /// only means something when we're talking about content...
        /// </summary>
        public string Stereotype { get; set; }

        /// <summary>
        /// Legacy from Content shapes
        /// TODO: Figure out a useful way to do something with this (workflows?)
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Context from a parent shape operation (if there was one)
        /// </summary>
        public ModelShapeContext ParentContext { get; set; }

        public IDictionary<Type, object> CustomContext { get; set; }

        /// <summary>
        /// New and best method for storing arbitrary custom values
        /// </summary>
        public dynamic Stuff { get; set; }

        /// <summary>
        /// Contexts inserted here will be executed after the current drivers have run, allowing
        /// a different model to get run over the same base shape.
        /// TODO: What are we actually using this for and is there a reason why it's not practical
        /// to just call Origami again on the rare occasions when this is actually required?
        /// </summary>
        public List<ModelChainContext> ChainedResults { get; set; }

        /// <summary>
        /// Handlers to be called after an update operation
        /// TODO: Make this clearer in function and add other handlers if we need this pattern.
        /// Or, use a better pattern.
        /// </summary>
        protected List<Action<ModelShapeContext>> UpdatedHandlers = new List<Action<ModelShapeContext>>();

        /// <summary>
        /// Adds a handler to be called after update
        /// </summary>
        /// <param name="updated"></param>
        public void OnUpdated(Action<ModelShapeContext> updated) {
            UpdatedHandlers.Add(updated);
        }

        /// <summary>
        /// Invoke all update handlers
        /// </summary>
        public void InvokeUpdated() {
            foreach (var handler in UpdatedHandlers) {
                handler.Invoke(this);
            }
        }

        /// <summary>
        /// Gets all parent contexts from up the chain
        /// TODO: Make this an extension method
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModelShapeContext> ParentChain()
        {
            var currentParent = ParentContext;
            while (currentParent != null)
            {
                yield return currentParent;
                currentParent = currentParent.ParentContext;
            }
        }

        protected string CurrentDestination { get; set; }
        public object SetDestination(string dest) {
            if (dest != CurrentDestination) {
                CurrentDestination = dest;
                Shape = DestinationShapes.Get(dest);
                return Shape;
            }
            return null;
        }

        /// <summary>
        /// Perform an action if we have a particular custom context
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="action"></param>
        public void With<TContext>(Action<TContext> action) {
            if (CustomContext.ContainsKey(typeof(TContext))) {
                action((TContext)CustomContext[typeof(TContext)]);
            }
 //           if (Stuff.HasThing(typeof(TContext))) {
    //            action(Stuff.GetThing(typeof(TContext)));
       //     }

        }
    }
}