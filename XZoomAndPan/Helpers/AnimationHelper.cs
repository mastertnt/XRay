using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace XZoomAndPan.Helpers
{
    /// <summary>
    /// A helper class to simplify animation.
    /// </summary>
    public static class AnimationHelper
    {
        /// <summary>
        /// Starts an animation to a particular value on the specified dependency property.
        /// </summary>
        /// <param name="pAnimatableElement">The gui element to animate.</param>
        /// <param name="pDependencyProperty">The dependency property to animate.</param>
        /// <param name="pToValue">The final value of the dependency property.</param>
        /// <param name="pAnimationDurationSeconds">The animation duration.</param>
        public static void StartAnimation(UIElement pAnimatableElement, DependencyProperty pDependencyProperty, double pToValue, double pAnimationDurationSeconds)
        {
            StartAnimation(pAnimatableElement, pDependencyProperty, pToValue, pAnimationDurationSeconds, null);
        }

        /// <summary>
        /// Starts an animation to a particular value on the specified dependency property.
        /// You can pass in an event handler to call when the animation has completed.
        /// </summary>
        /// <param name="pAnimatableElement">The gui element to animate.</param>
        /// <param name="pDependencyProperty">The dependency property to animate.</param>
        /// <param name="pToValue">The final value of the dependency property.</param>
        /// <param name="pAnimationDurationSeconds">The animation duration.</param>
        /// <param name="pCompletedEvent">The callback executed when the animation ended.</param>
        public static void StartAnimation(UIElement pAnimatableElement, DependencyProperty pDependencyProperty, double pToValue, double pAnimationDurationSeconds, EventHandler pCompletedEvent)
        {
            double lFromValue = (double)pAnimatableElement.GetValue(pDependencyProperty);

            DoubleAnimation lAnimation = new DoubleAnimation();
            lAnimation.From = lFromValue;
            lAnimation.To = pToValue;
            lAnimation.Duration = TimeSpan.FromSeconds(pAnimationDurationSeconds);

            lAnimation.Completed += delegate(object pSender, EventArgs pEventArgs)
            {
                //
                // When the animation has completed bake final value of the animation
                // into the property.
                //
                pAnimatableElement.SetValue(pDependencyProperty, pAnimatableElement.GetValue(pDependencyProperty));
                CancelAnimation(pAnimatableElement, pDependencyProperty);

                if (pCompletedEvent != null)
                {
                    pCompletedEvent(pSender, pEventArgs);
                }
            };

            lAnimation.Freeze();

            pAnimatableElement.BeginAnimation(pDependencyProperty, lAnimation);
        }

        /// <summary>
        /// Cancel any animations that are running on the specified dependency property.
        /// </summary>
        /// <param name="pAnimatableElement">The gui element to animate.</param>
        /// <param name="pDependencyProperty">The dependency property to animate.</param>
        public static void CancelAnimation(UIElement pAnimatableElement, DependencyProperty pDependencyProperty)
        {
            pAnimatableElement.BeginAnimation(pDependencyProperty, null);
        }
    }
}
