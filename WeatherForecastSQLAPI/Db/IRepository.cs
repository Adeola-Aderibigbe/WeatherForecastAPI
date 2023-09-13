namespace Practice_Docker.Db
{
    public interface IRepository
    {
        Task<bool> AddForecast(WeatherForecast weatherForecast);
        Task<bool> RemoveForecast(WeatherForecast weatherForecast);
        Task<bool> UpdateForecast(WeatherForecast weatherForecast);
        Task<WeatherForecast> GetWeatherForecast(int id);
        IEnumerable<WeatherForecast> GetAllForecasts();
    }
}
