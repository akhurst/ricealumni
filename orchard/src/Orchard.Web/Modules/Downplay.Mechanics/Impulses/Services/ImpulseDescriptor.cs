using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Security.Permissions;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Impulses.Services {
    public class ImpulseDescriptor {

        public ImpulseDescriptor(string name) {
            Name = name;
        }
        public string Name { get; private set; }

        public Orchard.Localization.LocalizedString Caption { get; set; }
        public Orchard.Localization.LocalizedString Description { get; set; }

        public List<String> ContentTypes = new List<string>();
        public List<String> RequiredScripts = new List<string>();
        public List<Permission> Permissions = new List<Permission>();

        public List<Action<ImpulseDisplayContext>> DisplayingHandlers = new List<Action<ImpulseDisplayContext>>();
        public List<Action<ImpulseContext>> ActuatingHandlers = new List<Action<ImpulseContext>>();
        public List<Func<IContent, bool>> ContentFilters = new List<Func<IContent, bool>>();

        public ImpulseDescriptor ForContentTypes(params string[] contentTypes) {
            ContentTypes.AddRange(contentTypes);
            return this;
        }

        public ImpulseDescriptor RequireScripts(params string[] scripts) {
            RequiredScripts.AddRange(scripts);
            return this;
        }

        public ImpulseDescriptor OnDisplaying(Action<ImpulseDisplayContext> displaying) {
            DisplayingHandlers.Add(displaying);
            return this;
        }

        public ImpulseDescriptor ForPermissions(params Orchard.Security.Permissions.Permission[] permissions) {
            Permissions.AddRange(permissions);
            return this;
        }

        public ImpulseDescriptor ForPart<T>(Action<ImpulseDisplayContext, T> display = null, Action<ImpulseContext, T> actuate = null) where T:IContent {
            ContentFilters.Add((c) => c.Has<T>());
            if (display != null)
                DisplayingHandlers.Add((c) => display(c, c.Content.As<T>()));
            if (actuate != null)
                ActuatingHandlers.Add((c) => actuate(c, c.SourceContent.As<T>()));
            return this;
        }

        public void Actuate(ImpulseContext context) {
            foreach (var h in ActuatingHandlers) {
                h(context);
            }
        }
    }
}
