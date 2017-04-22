using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DotNetKit.Windows.Media
{
    public static class VisualTreeHelperExtension
    {
        #region VisualChildren
        public struct VisualChildrenEnumerator
            : IEnumerator<DependencyObject>
        {
            readonly DependencyObject obj;
            int index;
            int count;

            public DependencyObject Current => VisualTreeHelper.GetChild(obj, index);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < count;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
            }

            public VisualChildrenEnumerator(DependencyObject obj)
            {
                this.obj = obj;
                index = 0;
                count = VisualTreeHelper.GetChildrenCount(obj);
            }
        }

        public struct VisualChildrenEnumerable
            : IEnumerable<DependencyObject>
        {
            readonly DependencyObject obj;

            public VisualChildrenEnumerator GetEnumerator() =>
                new VisualChildrenEnumerator(obj);

            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();

            IEnumerator<DependencyObject> IEnumerable<DependencyObject>.GetEnumerator() =>
                GetEnumerator();

            public VisualChildrenEnumerable(DependencyObject obj)
            {
                this.obj = obj;
            }
        }

        public static VisualChildrenEnumerable VisualChildren(this DependencyObject obj)
        {
            return new VisualChildrenEnumerable(obj);
        }
        #endregion

        public static IEnumerable<DependencyObject>
            VisualDescendants(this DependencyObject obj)
        {
            var stack = new Stack<DependencyObject>();
            stack.Push(obj);

            while (stack.Count > 0)
            {
                var it = stack.Pop();
                yield return it;

                foreach (var child in it.VisualChildren())
                {
                    stack.Push(child);
                }
            }
        }
    }
}
