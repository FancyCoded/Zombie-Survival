using UnityEngine.Events;

public interface IRealoadable
{
    int MagazineSize { get; }
    int AmmunitionCountInMagazine { get; }
    string AmmunitionName { get; }
    
    UnityAction<int> AmmunitionCountInMagazineChanged { get; set; }

    void Reload();
    void PlayDryFire();
}