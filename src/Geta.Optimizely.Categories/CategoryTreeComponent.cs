// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

using Geta.Optimizely.Categories.Core;

namespace Geta.Optimizely.Categories
{
    [Component]
    public class CategoryTreeComponent : ComponentDefinitionBase
    {
        private string _title;
        private string _description;

        public CategoryTreeComponent() : this(ServiceLocator.Current.GetInstance<CategorySettings>())
        {
        }

        public CategoryTreeComponent(CategorySettings categorySettings)
            : base("epi-cms/component/MainNavigationComponent")
        {
            LanguagePath = "/getacategories/treecomponent";

            Categories = new[] { "content" };
            PlugInAreas = new []
            {
                PlugInArea.AssetsDefaultGroup
            };

            SortOrder = 105;
            Settings.Add(new Setting("repositoryKey", CategoryContentRepositoryDescriptor.RepositoryKey));
        }

        public override string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_title))
                {
                    return _title;
                }

                return LocalizationService.GetString($"{LanguagePath}/title", "Categories");
            }
            set => _title = value;
        }

        public override string Description
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_description))
                {
                    return _description;
                }

                return LocalizationService.GetString($"{LanguagePath}/description", "Category management");
            }
            set => _description = value;
        }
    }
}
