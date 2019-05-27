using Sitecore.Diagnostics;
using Sitecore.Publishing;
using PipelinePublishProviderOrigin = Sitecore.Publishing.PipelinePublishProvider;
using PublishHelperOrigin = Sitecore.Publishing.PublishHelper;
using SupportPublishHelper = Sitecore.Support.Publishing.PublishHelper;

namespace Sitecore.Support.Publishing
{
    public class PipelinePublishProvider : PipelinePublishProviderOrigin
    {
        public override PublishHelperOrigin CreatePublishHelper(PublishOptions options)
        {
            Assert.ArgumentNotNull(options, "options");
            return new SupportPublishHelper(options);
        }
    }
}