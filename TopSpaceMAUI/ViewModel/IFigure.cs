namespace TopSpaceMAUI.ViewModel
{
    public interface IFigure
    {
        void Draw(ICanvas canvas, RectF rectF);
        Task Configure();
    }

}