using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using System.Reflection;
using PublishHelperOrigin = Sitecore.Publishing.PublishHelper;

namespace Sitecore.Support.Publishing
{
    public class PublishHelper : PublishHelperOrigin
    {
        public PublishHelper(PublishOptions options) : base (options)
        {
        }

        public override void CopyToTarget(Item sourceVersion, Item originalItem)
        {
            Assert.ArgumentNotNull(sourceVersion, "sourceVersion");
            using (new SecurityDisabler())
            {
                bool flag = true;
                var transformToTargetVersion = typeof(PublishHelperOrigin).GetMethod("TransformToTargetVersion", BindingFlags.NonPublic | BindingFlags.Instance);
                Item targetVersion = transformToTargetVersion.Invoke(this, new object[] { sourceVersion }) as Item;
                if (originalItem != null)
                {
                    ItemChanges comparedChanges = this.UpdateTargetItemChanges(targetVersion, originalItem);
                    flag = this.IsNewVersionToPublish(comparedChanges, targetVersion, originalItem);

                    if (flag)
                    {
                        var getFullChanges = typeof(Item).GetMethod("GetFullChanges", BindingFlags.NonPublic | BindingFlags.Instance);
                        comparedChanges = getFullChanges.Invoke(targetVersion, null) as ItemChanges;
                    }
                    var setChanges = typeof(Item).GetMethod("SetChanges", BindingFlags.NonPublic | BindingFlags.Instance);
                    setChanges.Invoke(targetVersion, new object[] { comparedChanges });
                }
                using (new EditContext(targetVersion))
                {
                    targetVersion.RuntimeSettings.SaveAll = flag;
                }
            }
        }

        private bool IsNewVersionToPublish(ItemChanges comparedChanges, Item targetVersion, Item orginalItem)
        {
            return ((comparedChanges.FieldChanges.Count == 0 && !comparedChanges.Renamed) || targetVersion.Version != orginalItem.Version);
        }

    }
}