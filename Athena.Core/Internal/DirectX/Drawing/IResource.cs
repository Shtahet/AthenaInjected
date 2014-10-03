namespace Athena.Core.Internal.DirectX.Drawing
{
    public interface IResource
    {
        void Draw();

        bool Remove { get; set; }
        void OnBeforeRemove();
    }
}
