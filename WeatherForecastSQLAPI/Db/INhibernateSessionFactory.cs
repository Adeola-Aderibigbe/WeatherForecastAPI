namespace Practice_Docker.Db
{
    public interface INhibernateSessionFactory
    {
        NHibernate.ISession GetSession { get; }
    }
}
