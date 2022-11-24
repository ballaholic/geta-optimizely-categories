// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Globalization;
using Geta.Optimizely.Categories.Core.Extensions;

namespace Geta.Optimizely.Categories.Core
{
    public class DefaultContentInCategoryLocator : IContentInCategoryLocator
    {
        protected readonly IContentRepository ContentRepository;
        protected readonly LanguageResolver LanguageResolver;

        public DefaultContentInCategoryLocator(IContentRepository contentRepository, LanguageResolver languageResolver)
        {
            ContentRepository = contentRepository;
            LanguageResolver = languageResolver;
        }

        public virtual IEnumerable<T> GetChildren<T>(ContentReference contentLink, IEnumerable<ContentReference> categories) where T : ICategorizableContent, IContentData
        {
            return GetChildren<T>(contentLink, categories, CreateDefaultListLoaderOptions());
        }

        public virtual IEnumerable<T> GetChildren<T>(ContentReference contentLink, IEnumerable<ContentReference> categories, CultureInfo culture) where T : ICategorizableContent, IContentData
        {
            var loaderOptions = new LoaderOptions { LanguageLoaderOption.Specific(culture) };
            return GetChildren<T>(contentLink, categories, loaderOptions);
        }

        public virtual IEnumerable<T> GetChildren<T>(ContentReference contentLink, IEnumerable<ContentReference> categories, LoaderOptions loaderOptions) where T : ICategorizableContent, IContentData
        {
            return ContentRepository
                .GetChildren<T>(contentLink, loaderOptions)
                .Where(x => x.Categories.MemberOfAny(categories));
        }

        public virtual IEnumerable<T> GetDescendents<T>(ContentReference contentLink, IEnumerable<ContentReference> categories) where T : ICategorizableContent, IContentData
        {
            return GetDescendents<T>(contentLink, categories, CreateDefaultListLoaderOptions());
        }

        public virtual IEnumerable<T> GetDescendents<T>(ContentReference contentLink, IEnumerable<ContentReference> categories, CultureInfo culture) where T : ICategorizableContent, IContentData
        {
            var loaderOptions = new LoaderOptions { LanguageLoaderOption.Specific(culture) };
            return GetDescendents<T>(contentLink, categories, loaderOptions);
        }

        public virtual IEnumerable<T> GetDescendents<T>(ContentReference contentLink, IEnumerable<ContentReference> categories, LoaderOptions loaderOptions) where T : ICategorizableContent, IContentData
        {
            var contentLinks = ContentRepository.GetDescendents(contentLink);

            return ContentRepository
                .GetItems(contentLinks, loaderOptions)
                .OfType<T>()
                .Where(x => x.Categories.MemberOfAny(categories));
        }

        public virtual IEnumerable<T> GetReferencesToCategories<T>(IEnumerable<ContentReference> categories) where T : ICategorizableContent, IContentData
        {
            return GetReferencesToCategories<T>(categories, CreateDefaultListLoaderOptions());
        }

        public virtual IEnumerable<T> GetReferencesToCategories<T>(IEnumerable<ContentReference> categories, CultureInfo culture) where T : ICategorizableContent, IContentData
        {
            var loaderOptions = new LoaderOptions { LanguageLoaderOption.Specific(culture) };
            return GetReferencesToCategories<T>(categories, loaderOptions);
        }

        public virtual IEnumerable<T> GetReferencesToCategories<T>(IEnumerable<ContentReference> categories, LoaderOptions loaderOptions) where T : ICategorizableContent, IContentData
        {
            if (categories != null && categories.Any())
            {
                var referenceContentLinks = new List<ContentReference>();

                foreach (var category in categories)
                {
                    var referencesToContent = ContentRepository.GetReferencesToContent(category, false);
                    referenceContentLinks.AddRange(referencesToContent.Select(x => x.OwnerID));
                }

                return ContentRepository
                    .GetItems(referenceContentLinks.Distinct(), loaderOptions)
                    .OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        protected virtual LoaderOptions CreateDefaultListLoaderOptions()
        {
            return new LoaderOptions
            {
                LanguageLoaderOption.Fallback(LanguageResolver.GetPreferredCulture())
            };
        }
    }
}
