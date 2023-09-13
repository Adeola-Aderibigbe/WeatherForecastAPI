using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Practice_Docker.Db.Mappings;

namespace Practice_Docker.Db
{
    public class SessionFactory : INhibernateSessionFactory
    {
        public SessionFactory(IConfiguration config)
        {
            Enum.TryParse(config["DatabaseType"], out DatabaseType type);

            var _connectionString = type == DatabaseType.Mssql ? config.GetConnectionString("WeatherForecast") : config.GetConnectionString("WeatherForecastLite");
            if (string.IsNullOrEmpty(_connectionString))
                throw new NullReferenceException("Connection string cannot be null");

           

            _sessionFactory = InitialiseSessionFactory(_connectionString, type);
        }

       
        public NHibernate.ISession GetSession => _sessionFactory.OpenSession();


        private ISessionFactory InitialiseSessionFactory(string connectionString, DatabaseType type)
        {
            return type switch
            {
                DatabaseType.Mssql => BuildMsSqlSessionFactory(connectionString),
                DatabaseType.Sqlite => BuildSqliteSessionFactory(connectionString),
                _ => BuildMsSqlSessionFactory(connectionString)
            };
        }
        private ISessionFactory BuildMsSqlSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)
                .ShowSql()
                .FormatSql())
                .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(WeatherForecastMapping).Assembly))
                .ExposeConfiguration(cfg =>
                {
                    new SchemaUpdate(cfg).Execute(true, true);
                });
            return configuration.BuildSessionFactory();
        }
        private ISessionFactory BuildSqliteSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(connectionString)
                .ShowSql()
                .FormatSql())
                .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(WeatherForecastMapping).Assembly))
                .ExposeConfiguration(cfg =>
                {
                    new SchemaUpdate(cfg).Execute(true, true);
                });
            return configuration.BuildSessionFactory();
        }

        private readonly ISessionFactory _sessionFactory;
    }
}
