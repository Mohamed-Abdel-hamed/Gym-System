namespace Gym.Api.Abstractions.Consts;

public enum MembershipStatus
{
    Active,  //Member can access services
    Paused,//Temporarily paused (manual or admin hold)
    Expired,//EndDate passed, not renewed or paid again
    Cancelled,//Membership ended early
    Freeze //Member is using allowed freeze days (based on plan)
}