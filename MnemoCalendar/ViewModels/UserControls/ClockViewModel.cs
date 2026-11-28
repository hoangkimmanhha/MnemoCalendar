using CommunityToolkit.Mvvm.ComponentModel;
using MnemoCalendar.Helpers;
using System.Windows.Threading;

namespace MnemoCalendar.ViewModels.UserControls;

public partial class ClockViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Greeting))]
    [NotifyPropertyChangedFor(nameof(TimeIconPath))]
    [NotifyPropertyChangedFor(nameof(DotVisibility))]
    private DateTimeOffset _currentTime = DateTimeOffset.Now;

    public string Greeting => GreetingHelper(CurrentTime);
    public string TimeIconPath => IconHelper(CurrentTime);
    public string DotVisibility => CurrentTime.Millisecond < 500 ? "Visible" : "Collapsed";

    public ClockViewModel()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _timer.Tick += (_, _) => {
            CurrentTime = DateTimeOffset.Now;
        };
        _timer.Start();
    }
    private string GreetingHelper(DateTimeOffset currentTime)
    {
        var hour = currentTime.Hour;
        if (hour >= 18) return "Good evening!";
        if (hour >= 12) return "Good afternoon!";
        return "Good morning!";
    }
    private string IconHelper(DateTimeOffset currentTime)
    {
        var hour = currentTime.Hour;
        if (hour >= 18) return "/Assets/Media/night.png";
        if (hour >= 12) return "/Assets/Media/afternoon.png";
        return "/Assets/Media/morning.png";
    }
}