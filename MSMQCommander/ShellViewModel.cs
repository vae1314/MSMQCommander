using System.Collections.ObjectModel;
using Caliburn.Micro;
using MSMQCommander.Events;
using MSMQCommander.Views;
using System.ComponentModel.Composition;
using System.Linq;

namespace MSMQCommander 
{
    [Export(typeof(IShell))]
    public class ShellViewModel : 
        PropertyChangedBase, 
        IShell,
        IHandle<QueueSelectedEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ObservableCollection<DetailsView> _detailsViews = new ObservableCollection<DetailsView>();

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public ObservableCollection<DetailsView> DetailsViews
        {
            get { return _detailsViews; }
        }

        public void Handle(QueueSelectedEvent message)
        {
            var existingViewForQueue = DetailsViews.SingleOrDefault(d => d.Title == message.MessageQueue.QueueName);
            if (existingViewForQueue != null)
            {
                existingViewForQueue.Activate();
            }
            else
            {
                var newDetailsView = new DetailsView {Title = message.MessageQueue.QueueName};
                DetailsViews.Add(newDetailsView);
                NotifyOfPropertyChange(() => DetailsViews);
                newDetailsView.Activate();
            }
        }
    }
}
