using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.InnerMessages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ParticipantsViewModel : ViewModelBase
    {
        //private readonly IUserLoader _userLoader;

        //private ObservableCollection<ParticipantObservable> _participants;
        //public ObservableCollection<ParticipantObservable> Participants
        //{
        //    get => _participants;
        //    private set
        //    {
        //        Set(ref _participants, value);
        //        RaisePropertyChanged(nameof(ParticipantOnlineCount));
        //    }
        //}
        //public int ParticipantOnlineCount => Participants.Count(p => p.IsOnline);

        //private bool _isLoaded = false;
        //public bool IsLoaded
        //{
        //    get => _isLoaded;
        //    private set => Set(ref _isLoaded, value);
        //}


        //public ParticipantsViewModel(IUserLoader userLoader)
        //{
        //    _userLoader = userLoader;
        //    Participants.CollectionChanged += (o, e) => RaisePropertyChanged(nameof(ParticipantOnlineCount));
        //    this.MessengerInstance.Register<ParticipantStatusChangedIMessage>(this, OnParticipantLoggedIn);
            
        //}

        //public override void Cleanup()
        //{
        //    base.Cleanup();
        //    this.MessengerInstance.Unregister<ParticipantStatusChangedIMessage>(this);
        //    _isLoaded = false;
        //}


        //private void OnParticipantLoggedIn(ParticipantStatusChangedIMessage message)
        //{
        //    var user = Participants.FirstOrDefault();
        //    if (user == null)
        //        Participants.Add(new ParticipantObservable(message.UserId, true, message.UpdateDateTime));
        //    else
        //    {
        //        user.ChangeStatus(true, message.UpdateDateTime);
        //        RaisePropertyChanged(nameof(ParticipantOnlineCount));
        //    }
        //}

        


        //private async Task LoadParticipants()
        //{
        //    _ = await _userLoader.GetUsers();
        //}
    }
}
