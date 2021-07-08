using Oyshroom.Data.Model;


namespace Oyshroom.Data
{
    public class FuzzyLogicRepository : Repository<FuzzyLogic>
    {
        public FuzzyLogicRepository(Database database) : base(database)
        {

        }

        public FuzzyLogicRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }
    }
}
