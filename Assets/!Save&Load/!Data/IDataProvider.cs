public interface IDataProvider
{
    void Save(int slotId);
    void Delete(int slotId);
    bool TryLoad(int slotId);
}
