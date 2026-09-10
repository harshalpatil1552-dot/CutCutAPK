using CutCutAPK.Models.Bookings;

namespace CutCutAPK.Views.Shared;

/// <inheritdoc cref="StatusBadgeView" path="/summary" />
public partial class StatusBadgeView : ContentView
{
    public static readonly BindableProperty StatusIdProperty = BindableProperty.Create(
        nameof(StatusId),
        typeof(BookingStatusId?),
        typeof(StatusBadgeView),
        propertyChanged: OnStatusIdChanged);

    public BookingStatusId? StatusId
    {
        get => (BookingStatusId?)GetValue(StatusIdProperty);
        set => SetValue(StatusIdProperty, value);
    }

    public StatusBadgeView()
    {
        InitializeComponent();
        Apply(null);
    }

    private static void OnStatusIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((StatusBadgeView)bindable).Apply((BookingStatusId?)newValue);
    }

    /// <summary>Same status -> (label, tone) mapping as the web app's StatusBadgeComponent.</summary>
    private void Apply(BookingStatusId? statusId)
    {
        var (label, background, text) = statusId switch
        {
            BookingStatusId.Pending => ("Pending", Color.FromArgb("#FEF3C7"), Color.FromArgb("#92400E")),
            BookingStatusId.Confirmed => ("Confirmed", Color.FromArgb("#DBEAFE"), Color.FromArgb("#1E40AF")),
            BookingStatusId.CheckedIn => ("Checked In", Color.FromArgb("#E0E7FF"), Color.FromArgb("#3730A3")),
            BookingStatusId.InProgress => ("In Progress", Color.FromArgb("#E0E7FF"), Color.FromArgb("#3730A3")),
            BookingStatusId.Completed => ("Completed", Color.FromArgb("#DCFCE7"), Color.FromArgb("#166534")),
            BookingStatusId.Cancelled => ("Cancelled", Color.FromArgb("#FEE2E2"), Color.FromArgb("#991B1B")),
            _ => ("Unknown", Color.FromArgb("#FEF3C7"), Color.FromArgb("#92400E")),
        };

        PillLabel.Text = label;
        PillLabel.TextColor = text;
        Pill.BackgroundColor = background;
    }
}
