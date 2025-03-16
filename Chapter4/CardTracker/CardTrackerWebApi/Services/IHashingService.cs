namespace CardTrackerWebApi.Services;

public interface IHashingService
{
    byte[] ComputeHash(string inputString, byte[] salt);
    byte[] GenerateSalt();
}