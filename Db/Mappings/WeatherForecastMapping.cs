using FluentNHibernate.Mapping;

namespace Practice_Docker.Db.Mappings
{
    public class WeatherForecastMapping : ClassMap<WeatherForecast>
    {
        public WeatherForecastMapping()
        {
            Id(s => s.Id);
            Map(s => s.Date);
            Map(s => s.Summary);
            Map(s => s.TemperatureC);
        }
    }
}
