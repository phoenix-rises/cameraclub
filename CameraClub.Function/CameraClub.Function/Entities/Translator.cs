using CameraClub.Function.Contracts;

namespace CameraClub.Function.Entities
{
    public class Translator
    {
        public void TranslateCategory(UpsertCategoryRequest request, Category entity)
        {
            entity.Name = request.Name;
            entity.IsDigital = request.IsDigital;
        }

        public void TranslateCompetition(UpsertCompetitionRequest request, Competition entity)
        {
            entity.Name = request.Name;
            entity.Date = request.Date;
            entity.HasDigital = request.HasDigital;
            entity.HasPrint = request.HasPrint;
        }

        public void TranslateJudge(UpsertJudgeRequest request, Judge entity)
        {
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Email = request.Email;
            entity.Bio = request.Bio;
            entity.PhoneNumber = request.PhoneNumber;
        }

        public void TranslatePhotographer(UpsertPhotographerRequest request, Photographer entity)
        {
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Email = request.Email;
            entity.CompetitionNumber = request.CompetitionNumber;
            entity.ClubNumber = request.ClubNumber;
        }

        public void TranslateClub(UpsertClubRequest request, Club entity)
        {
            entity.Name = request.Name;
            entity.ContactName = request.ContactName;
            entity.ContactEmail = request.ContactEmail;
            entity.ClubAssocationNumber = request.ClubAssociationNumber;
        }
    }
}