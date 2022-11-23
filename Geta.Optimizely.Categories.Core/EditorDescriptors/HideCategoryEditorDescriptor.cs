// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Geta.Optimizely.Categories.Core.Configuration;
using Microsoft.Extensions.Options;

namespace Geta.Optimizely.Categories.Core.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(CategoryList))]
    public class HideCategoryEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var configuration = ServiceLocator.Current.GetInstance<IOptions<CategoriesOptions>>().Value;
            var showDefaultCategoryProperty = configuration.ShowDefaultCategoryProperty;

            if (showDefaultCategoryProperty || !metadata.PropertyName.Equals("icategorizable_category", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            metadata.ShowForEdit = false;
            metadata.ShowForDisplay = false;
        }
    }
}
