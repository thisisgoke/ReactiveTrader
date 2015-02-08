using System;
using System.Collections.ObjectModel;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Support.V7.Widget;
using Android.Views;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollection;

        private readonly IDisposable _collectionChangedSubscription;

        public SpotTileAdapter(ObservableCollection<ISpotTileViewModel> spotTileCollection)
        {
            _spotTileCollection = spotTileCollection;

            _collectionChangedSubscription = _spotTileCollection.ObserveCollection()
                .Subscribe(_ =>
                {
                    NotifyDataSetChanged(); // xamtodo - make the change details more explicit and move to some common code
                });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var spotTileViewModel = _spotTileCollection[position];
            if (spotTileViewModel.CurrencyPair == null)
            {
                return;
            }

            var viewHolder = (SpotTileViewHolder)holder;
            viewHolder.CurrencyPairLabel.Text = spotTileViewModel.CurrencyPair;
            viewHolder.BidButton.SetDataContext(spotTileViewModel.Pricing.Bid);
            viewHolder.AskButton.SetDataContext(spotTileViewModel.Pricing.Ask);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SpotTileView, parent, false);
            var holder = new SpotTileViewHolder(v);
            return holder;
        }

        public override int ItemCount
        {
            get { return _spotTileCollection.Count; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _collectionChangedSubscription.Dispose();
            }
        }
    }
}