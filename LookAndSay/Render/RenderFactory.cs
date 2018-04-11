namespace LookAndSay.Render
{
    public enum RenderType {Console};

    public static class RenderFactory
    {
        public static IRenderResults Create(RenderType type)
        {
            switch (type)
            {
                case RenderType.Console:
                    return new ConsoleRender();
            }

            return null;
        }
    }
}