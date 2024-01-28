using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WeatherAppWPF.Model;
using WeatherAppWPF.ViewModel.Commands;
using WeatherAppWPF.ViewModel.Helpers;

namespace WeatherAppWPF.ViewModel;

public class WeatherVM : INotifyPropertyChanged
{
    private string _query;
    private CurrentConditions _currentConditions;
    private City _selectedCity;

    public string Query
    {
        get { return _query; }
        set
        {
            _query = value;
            OnPropertyChanged("Query");
        }
    }

    public CurrentConditions CurrentConditions
    {
        get { return _currentConditions; }
        set
        {
            _currentConditions = value;
            OnPropertyChanged("CurrentConditions");
        }
    }

    public City SelectedCity

    {
        get { return _selectedCity; }
        set
        {
            _selectedCity = value;
            OnPropertyChanged("SelectedCity");
            GetCurrentConditions();
        }
    }

    public SearchCommand SearchCommand { get; set; }
    public ObservableCollection<City> Cities { get; set; }

    public WeatherVM()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            SelectedCity = new City
            {
                LocalizedName = "Krakow"
            };

            CurrentConditions = new CurrentConditions
            {
                WeatherText = "Partly cloudy",
                Temperature = new Temperature
                {
                    Metric = new Units
                    {
                        Value = "21"
                    }
                }
            };
        }

        SearchCommand = new SearchCommand(this);
        Cities = new ObservableCollection<City>();
    }

    private async void GetCurrentConditions()
    {
        Query = string.Empty;
        Cities.Clear();
        CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
    }

    public async void MakeQuery()
    {
        var cities = await AccuWeatherHelper.GetCities(Query);

        Cities.Clear();

        foreach (var city in cities)
        {
            Cities.Add(city);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}