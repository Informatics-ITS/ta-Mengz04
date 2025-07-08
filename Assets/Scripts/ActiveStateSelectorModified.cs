/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus.Interaction
{
    /// <summary>
    /// Selects and unselects based on the Active State. If this component is piped into the Selector property of the Interactor component, it can replace poses for existing interactors with custom poses.
    /// </summary>
    public class ActiveStateSelectorModified : MonoBehaviour, ISelector
    {
        /// <summary>
        /// ISelector events will be raised based on state changes of this IActiveState.
        /// </summary>
        [Tooltip("ISelector events will be raised " +
            "based on state changes of this IActiveState.")]
        [SerializeField, Interface(typeof(IActiveState))]
        private UnityEngine.Object _activeState;
        protected IActiveState ActiveState { get; private set; }

        private bool _selecting = false;

        public event Action WhenSelected = delegate { };
        public event Action WhenUnselected = delegate { };

        [SerializeField]
        private OVRHand leftHand;

        private bool wasPinchingLastFrame = false;

        protected virtual void Awake()
        {
            ActiveState = _activeState as IActiveState;
        }

        protected virtual void Start()
        {
            this.AssertField(ActiveState, nameof(ActiveState));
        }

        protected virtual void Update()
        {
            bool isPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

            if (_selecting != ActiveState.Active && isPinching && !wasPinchingLastFrame)
            {
                _selecting = ActiveState.Active;
                if (_selecting)
                {
                    WhenSelected();
                }
            }

            // Pinch released this frame

            wasPinchingLastFrame = isPinching;
        }

        #region Inject

        public void InjectAllActiveStateSelector(IActiveState activeState)
        {
            InjectActiveState(activeState);
        }

        public void InjectActiveState(IActiveState activeState)
        {
            _activeState = activeState as UnityEngine.Object;
            ActiveState = activeState;
        }
        #endregion
    }
}
