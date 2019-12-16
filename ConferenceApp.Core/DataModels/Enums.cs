using System.ComponentModel.DataAnnotations;

namespace ConferenceApp.Core.DataModels
{
    public enum ReportStatus
    {
        [Display( Name = "Отсутствует" )] 
        None = 0,
        [Display( Name = "Утверждено" )] 
        Approved = 1,
        [Display( Name = "Отклонено" )]
        Rejected = 2
    }

    public enum RequestStatus
    {
        [Display( Name = "Отсутствует" )] 
        None = 0,
        [Display( Name = "Утверждено" )] 
        Approved = 1,
        [Display( Name = "Отклонено" )]
        Rejected = 2
    }

    public enum ReportType
    {
        [Display(Name = "Пленарный")]
        Plenary = 0,
        [Display(Name = "Секционный")]
        Sectional = 1,
        [Display(Name = "Стендовый")]
        Bench = 2,
        [Display(Name = "Публикация в сборнике")]
        PublicationInTheCollection = 3
    }

    public enum Degree
    {
        [Display(Name = "Бакалавр")]
        Bachelor = 0,
        [Display(Name = "Магистр")]
        Master = 1,
        [Display(Name = "Специалист")]
        Specialist = 2,
        [Display(Name = "Кандидат наук")]
        PhD = 3,
        [Display(Name = "Доктор наук")]
        ScD = 4
    }

    public enum Role
    {
        [Display(Name = "Пользователь")]
        User = 0,
        [Display(Name = "Модератор")]
        Moderator = 1
    }
}