using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Plugin.Maui.Audio;
using Weather.Models;
using Weather.Services.Interfaces;

namespace Weather.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private const string FailureSoundFileName = "fail.mp3";
    private const string SuccessSoundFileName = "success.mp3";
    private readonly IWeatherRestApi _weatherApi;
    private readonly IGeoLocationService _geoLocationService;
    private readonly IAudioManager _audioManager;

    private string _area;
    private ObservableCollection<WeatherInfo> _weatherLocations;
    private bool _isBusy;

    public MainViewModel(IWeatherRestApi weatherApi, IGeoLocationService geoLocationService, IAudioManager audioManager)
    {
        _weatherApi = weatherApi ?? throw new ArgumentNullException(nameof(weatherApi));
        _geoLocationService = geoLocationService ?? throw new ArgumentNullException(nameof(geoLocationService));
        _audioManager = audioManager;
        WeatherLocations = new ObservableCollection<WeatherInfo>();

        FetchWeatherCommand = new Command(async () => await FetchWeatherAsync(), () => !IsBusy);
        UseLocationCommand = new Command(async () => await UseLocationAsync(), () => !IsBusy);
    }

    public string Area
    {
        get => _area;
        set
        {
            if (_area == value)
            {
                return;
            }

            _area = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<WeatherInfo> WeatherLocations
    {
        get => _weatherLocations;
        private set
        {
            if (_weatherLocations == value)
            {
                return;
            }

            _weatherLocations = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (_isBusy == value)
            {
                return;
            }

            _isBusy = value;
            OnPropertyChanged();
            ((Command)FetchWeatherCommand).ChangeCanExecute();
            ((Command)UseLocationCommand).ChangeCanExecute();
        }
    }

    public ICommand FetchWeatherCommand { get; }
    public ICommand UseLocationCommand { get; }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private async Task FetchWeatherAsync()
    {
        if (string.IsNullOrWhiteSpace(Area)) return;

        IsBusy = true;
        try
        {
            var newLocation = await _weatherApi.GetWeatherAsync(Area);
            WeatherLocations.Add(newLocation);
            await PlaySound(SuccessSoundFileName);
        }
        catch
        {
            await PlaySound(FailureSoundFileName);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task UseLocationAsync()
    {
        IsBusy = true;
        try
        {
            var locationArea = await _geoLocationService.GetAreaFromLocationAsync();
            if (!string.IsNullOrWhiteSpace(locationArea))
            {
                Area = locationArea;
            }
            else
            {
                //Failed
            }
        }
        catch
        {
            //Log
            //Failed
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task PlaySound(string filename)
    {
        var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(filename));
        player.Play();
    }
}