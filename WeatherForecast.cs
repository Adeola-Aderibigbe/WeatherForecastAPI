namespace Practice_Docker
{
    public class WeatherForecast
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }

        public virtual int TemperatureC { get; set; }

        public virtual int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public virtual string? Summary { get; set; }
    }
}