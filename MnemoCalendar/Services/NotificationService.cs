using Microsoft.Toolkit.Uwp.Notifications;
using MnemoCalendar.Models;
using MnemoCalendar.Helpers;
using System.IO;
using System.Windows;

namespace MnemoCalendar.Services;

public class NotificationService
{
    /// <summary>
    /// Schedule new event notification if does not exist, otherwise update it.
    /// </summary>
    public static void ScheduleEventNotification(CalendarEvent calendarEvent)
    {
        CancelScheduledEventNotification(calendarEvent); // Try to remove it from scheduled event list first.
        if (calendarEvent.ReminderMinute is int reminderMin){
            new ToastContentBuilder()
                .AddText(calendarEvent.Title)
                .AddText($"*\tStart at: {calendarEvent.StartAt.LocalDateTime} \n*\tEnd at: {calendarEvent.EndAt.LocalDateTime} \n*\tLocation: {calendarEvent.Location ?? ""}")
                .AddAttributionText(calendarEvent.Description ?? "")
                .AddAppLogoOverride(new Uri($"file:///{PathHelper.BaseDirectory("Assets", "Media", "icon.png")}"))
                .SetToastScenario(ToastScenario.Alarm)
                .AddAudio(new ToastAudio
                {
                    Loop = true,
                    Src = new Uri(GetRingtoneString(ConfigReader.Parse("ringtone.current")!))
                })
                .Schedule(calendarEvent.StartAt.AddMinutes(reminderMin), toast =>
                {
                    toast.Tag = calendarEvent.Id.ToString(); // Use event ID as the ID for scheduled events.
                });
        }
    }
    public static bool CancelScheduledEventNotification(CalendarEvent calendarEvent)
    {
        var notifier = ToastNotificationManagerCompat.CreateToastNotifier();
        var scheduledToasts = notifier.GetScheduledToastNotifications(); // Get a list of all scheduled events.
        var toastToRemove = scheduledToasts.Where(t => t.Tag == calendarEvent.Id.ToString()).ToList(); // Filter it by ID.
        try
        {
            notifier.RemoveFromSchedule(toastToRemove[0]); // Only one event will be matched (if exists).
        }
        catch (ArgumentOutOfRangeException) {
            return false;
        }
        return true;
    }
    public static void CancelAllScheduledEventNotification()
    {
        var notifier = ToastNotificationManagerCompat.CreateToastNotifier();
        var scheduledToasts = notifier.GetScheduledToastNotifications(); // Get a list of all scheduled events.
        var toastToRemove = scheduledToasts.ToList();
        foreach (var t in toastToRemove) {
            notifier.RemoveFromSchedule(t);
        }
    }
    public static void ShowToast(string title, string message)
    {
        new ToastContentBuilder()
            .AddText(title)
            .AddText(message)
            .AddAppLogoOverride(new Uri($"file:///{PathHelper.BaseDirectory("Assests", "Media", "icon.png")}"))
            .Show();
    }
    public static void PlayRingtone(string ringtone)
    {
        try
        {
            GetRingtoneString(ringtone);
        }
        catch (FileNotFoundException) {
            MessageBox.Show("Invalid ringtone name. Only select system ringtones.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        new ToastContentBuilder()
            .AddText($"Playing {ringtone}")
            .AddAudio(new Uri(GetRingtoneString(ringtone)))
            .SetToastScenario(ToastScenario.Alarm)
            .AddAppLogoOverride(new Uri($"file:///{PathHelper.BaseDirectory("Assests", "Media", "icon.png")}"))
            .Show();
    }
    private static string GetRingtoneString(string ringtone) {
        if (ConfigReader.ParseList("ringtone.list").Contains(ringtone))
        {
            return $"ms-winsoundevent:Notification.Looping.{ringtone}";
        }
        else throw new FileNotFoundException();
    }
}