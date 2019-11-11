namespace ConferenceApp.Core.DataModels
{
    public enum ReportStatus
    {
        None = 0,
        Approved = 1,
        Rejected = 2
    }

    public enum RequestStatus
    {
        None = 0,
        Approved = 1,
        Rejected = 2
    }

    public enum ReportType
    {
        Plenary = 0,
        Sectional = 1,
        Bench = 2,
        PublicationInTheCollection = 3
    }

    public enum Degree
    {
        bachelor = 0,
        master = 1,
        specialist = 2,
        PhD = 3,
        ScD = 4
    }
}
