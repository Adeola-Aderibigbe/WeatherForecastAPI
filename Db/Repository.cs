using NHibernate.Linq;

namespace Practice_Docker.Db
{
    public class Repository : IRepository
    {
        public Repository(INhibernateSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetSession;
        }

        public async Task<bool> AddForecast(WeatherForecast weatherForecast)
        {
           var currentWeatherForecasts = _session.Query<WeatherForecast>().ToList();

           var currentWeatherForecast = currentWeatherForecasts.LastOrDefault();

           weatherForecast.Id = currentWeatherForecast != null ? currentWeatherForecast.Id + 1 : 0;
           await _session.SaveAsync(weatherForecast);
           return Commit();
        }

        public IEnumerable<WeatherForecast> GetAllForecasts()
        {
            return _session.Query<WeatherForecast>().OrderByDescending(s=>s.Date).ToList();
        }

        public async Task<WeatherForecast> GetWeatherForecast(int id)
        {
            return await _session.Query<WeatherForecast>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveForecast(WeatherForecast weatherForecast)
        {
            await _session.DeleteAsync(weatherForecast);
            return Commit();
        }

        public async Task<bool> UpdateForecast(WeatherForecast weatherForecast)
        {
            await _session.UpdateAsync(weatherForecast);
            return Commit();
        }


        private bool Commit()
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private readonly NHibernate.ISession _session;
    }
}
