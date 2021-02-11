using System;
using System.Threading.Tasks;

namespace CameraClub.Function
{
    public class UpsertEntity
    {
        public async Task<bool> Upsert<R, E>(CompetitionContext competitionContext, int? id, R request, Action<R, E> translate)
            where R : class
            where E : class, new()
        {
            E entity;

            if (id.HasValue)
            {
                entity = await competitionContext.FindAsync<E>(id.Value);

                if (entity == null)
                {
                    return false;
                }

                translate(request, entity);
            }
            else
            {
                entity = new E();
                translate(request, entity);

                competitionContext.Add(entity);
            }

            return true;
        }
    }
}