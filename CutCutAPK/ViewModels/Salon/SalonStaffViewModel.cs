using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Staff;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Salons;
using CutCutAPK.Services.Staff;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Mirrors features/salon/salon-staff/salon-staff.ts — look a registered SalonStaff user
/// up by phone, then add them to the selected salon's team.</summary>
public sealed partial class SalonStaffViewModel : SalonAreaViewModelBase
{
    private readonly IStaffService _staffService;

    public SalonStaffViewModel(
        IStaffService staffService,
        ISalonContextService salonContext,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService, salonContext)
    {
        _staffService = staffService;
    }

    public ObservableCollection<StaffResponseDto> StaffMembers { get; } = new();

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string? infoMessage;

    [ObservableProperty]
    private string lookupPhone = string.Empty;

    [ObservableProperty]
    private bool isLookingUp;

    [ObservableProperty]
    private StaffLookupResponseDto? foundUser;

    [ObservableProperty]
    private bool isAdding;

    public bool ShowEmptyState => !IsLoading && StaffMembers.Count == 0;

    public async Task LoadAsync()
    {
        if (!await EnsureSalonSelectedAsync())
        {
            return;
        }

        ErrorMessage = null;
        IsLoading = true;

        try
        {
            var staff = await _staffService.GetBySalonAsync(SalonContext.SelectedSalon!.SalonId);
            StaffMembers.Clear();
            foreach (var member in staff)
            {
                StaffMembers.Add(member);
            }
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(ShowEmptyState));
        }
    }

    [RelayCommand]
    private async Task LookupAsync()
    {
        ErrorMessage = null;
        InfoMessage = null;
        FoundUser = null;
        IsLookingUp = true;

        try
        {
            FoundUser = await _staffService.LookupByPhoneAsync(LookupPhone.Trim());
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsLookingUp = false;
        }
    }

    [RelayCommand]
    private async Task AddFoundUserAsync()
    {
        if (SalonContext.SelectedSalon is null || FoundUser is null)
        {
            return;
        }

        var user = FoundUser;
        IsAdding = true;

        try
        {
            var added = await _staffService.AddAsync(SalonContext.SelectedSalon.SalonId, new StaffCreateRequestDto { UserId = user.UserId });

            StaffMembers.Add(added);
            FoundUser = null;
            LookupPhone = string.Empty;
            InfoMessage = $"{user.FullName} added to your team.";
            OnPropertyChanged(nameof(ShowEmptyState));
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsAdding = false;
        }
    }

    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(ShowEmptyState));
}
