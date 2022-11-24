// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using Geta.Optimizely.Categories.Core.SelectionFactories;

namespace Geta.Optimizely.Categories.Core
{
    public class CategoryCriterionSettings : CriterionModelBase
    {
        [Required]
        [CriterionPropertyEditor(SelectionFactoryType = typeof(CategoryListing))]
        [DisplayName("Category")]
        public string CategoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [CriterionPropertyEditor(DefaultValue = 0)]
        [DisplayName("Viewed at least")]
        public int ViewedTimes { get; set; }

        public override ICriterionModel Copy() => ShallowCopy();
    }
}
