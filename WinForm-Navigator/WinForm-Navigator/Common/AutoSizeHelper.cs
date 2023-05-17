namespace Navigator.Common
{
    internal struct ScaleRate
    {
        public double xRate;
        public double yRate;
        public double wRate;
        public double hRate;
    }

    public class AutoSizeHelper
    {
        private ScrollableControl _container;

        private Dictionary<string, ScaleRate> scaleMap;

        //容器原设计大小集合
        private Dictionary<string, Size> ContainerDesignSizes;

        public AutoSizeHelper()
        {
            scaleMap = new Dictionary<string, ScaleRate>();
            ContainerDesignSizes = new Dictionary<string, Size>();
        }

        public void SetContainer(ScrollableControl container)
        {
            if (!(container is ContainerControl || container is Panel))
            {
                try
                {
                    throw new Exception("传入的容器类型不正确，请传入继承自ContainerControl或Panel的类");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return;
            }

            _container = container;

            Queue<Control> queue = new Queue<Control>();
            queue.Enqueue(_container);

            while (queue.Count > 0)
            {
                Control curCtrl = queue.Dequeue();

                foreach (Control ctrl in curCtrl.Controls)
                {
                    queue.Enqueue(ctrl);
                }

                //如果当前控件是容器，则加入到ContainerDesignSizes
                if (curCtrl is ContainerControl || curCtrl is Panel)
                {
                    ContainerDesignSizes.Add(curCtrl.Name,curCtrl.Size);
                }

                //对于一开始传入的容器不处理
                if (curCtrl == _container)
                {
                    continue;
                }

                //计算当前控件相对父容器的大小和位置比例，存储入map中
                var scaleRate = new ScaleRate
                {
                    xRate = curCtrl.Location.X * 1.0 / curCtrl.Parent.Width,
                    yRate = curCtrl.Location.Y * 1.0 / curCtrl.Parent.Height,
                    wRate = curCtrl.Width * 1.0 / curCtrl.Parent.Width,
                    hRate = curCtrl.Height * 1.0 / curCtrl.Parent.Height
                };
                scaleMap[curCtrl.Name] = scaleRate;
            }

            _container.SizeChanged += (s, e) =>
            {
                UpdateControlSize();
            };
        }

        public void UpdateControlSize()
        {
            Queue<Control> queue = new Queue<Control>();
            queue.Enqueue(_container);

            while (queue.Count > 0)
            {
                Control curCtrl = queue.Dequeue();

                foreach (Control ctrl in curCtrl.Controls)
                {
                    queue.Enqueue(ctrl);
                }
                //对于一开始传入的容器不处理
                if (curCtrl == _container)
                {
                    continue;
                }

                if (scaleMap.ContainsKey(curCtrl.Name))
                {
                    //根据map中存储的当前控件大小和位置比例，还原大小和位置
                    var scaleRate = scaleMap[curCtrl.Name];
                    int newX = (int)Math.Round(scaleRate.xRate * curCtrl.Parent.Width);
                    int newY = (int)Math.Round(scaleRate.yRate * curCtrl.Parent.Height);
                    int newW = (int)Math.Round(scaleRate.wRate * curCtrl.Parent.Width);
                    int newH = (int)Math.Round(scaleRate.hRate * curCtrl.Parent.Height);

                    curCtrl.Width = newW;
                    curCtrl.Height = newH;
                    curCtrl.Location = new Point(newX, newY);
                    //Console.WriteLine("【UpdateControlSize】");
                    //Console.WriteLine($"{curCtrl.Name}   location:[{curCtrl.Location}]  size:[{curCtrl.Size}]");
                }
            }

            //刷新界面
            _container.Invalidate();
        }

        public void AddNewControl(Control ctrl)
        {
            //找到该控件的容器原设计大小
            if (ctrl.Parent != null)
            {
                string parentName = ctrl.Parent.Name;
                Size parentDesignSize = ContainerDesignSizes[parentName];
                //计算位置和大小比例，再加入到map中
                var scaleRate = new ScaleRate
                {
                    xRate = ctrl.Location.X * 1.0 / parentDesignSize.Width,
                    yRate = ctrl.Location.Y * 1.0 / parentDesignSize.Height,
                    wRate = ctrl.Width * 1.0 / parentDesignSize.Width,
                    hRate = ctrl.Height * 1.0 / parentDesignSize.Height
                };
                scaleMap[ctrl.Name] = scaleRate;
            }
        }
    }
}