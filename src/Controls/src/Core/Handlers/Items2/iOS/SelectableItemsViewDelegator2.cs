#nullable disable
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Controls.Handlers.Items2
{
	public class SelectableItemsViewDelegator2<TItemsView, TViewController> : ItemsViewDelegator2<TItemsView, TViewController>
		where TItemsView : SelectableItemsView
		where TViewController : SelectableItemsViewController2<TItemsView>
	{
		public SelectableItemsViewDelegator2(UICollectionViewLayout itemsViewLayout, TViewController ItemsViewController2)
			: base(itemsViewLayout, ItemsViewController2)
		{
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			ViewController?.ItemSelected(collectionView, indexPath);
		}

		public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			ViewController?.ItemDeselected(collectionView, indexPath);
		}

		#pragma warning disable RS0016 // Add public types and members to the declared API
		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
#pragma warning restore RS0016 // Add public types and members to the declared API
		{
			ViewController?.WillDisplayCell(collectionView, cell, indexPath);
		}
	}
}