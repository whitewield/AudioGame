using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Phonon
{
    public class DirectSimulator
    {
        public void Init()
        {
            GlobalContext globalContext = PhononSettings.GetGlobalContext();
            RenderingSettings renderingSettings = PhononSettings.GetRenderingSettings();

            directAttnLerp.Init(directAttnLerpFrames);

            inputFormat = PhononSettings.GetAudioConfiguration();
            outputFormat = PhononSettings.GetAudioConfiguration();

            if (PhononCore.iplCreateBinauralRenderer(globalContext, renderingSettings, null, ref binauralRenderer) != Error.None)
            {
                Debug.Log("Unable to create binaural renderer. Please check the log file for details.");
                return;
            }

            if (outputFormat.channelLayout == ChannelLayout.Stereo)
            {
                // Create object based binaural effect for direct sound if the output format is stereo.
                if (PhononCore.iplCreateBinauralEffect(binauralRenderer, inputFormat, outputFormat, ref directBinauralEffect) != Error.None)
                {
                    Debug.Log("Unable to create binaural effect. Please check the log file for details.");
                    return;
                }
            }

            if (outputFormat.channelLayout == ChannelLayout.Custom)
            {
                // Panning effect for direct sound (used for rendering only for custom speaker layout, otherwise use default Unity panning)
                if (PhononCore.iplCreatePanningEffect(binauralRenderer, inputFormat, outputFormat, ref directCustomPanningEffect) != Error.None)
                {
                    Debug.Log("Unable to create custom panning effect. Please check the log file for details.");
                    return;
                }
            }

        }

        public void Destroy()
        {
            directAttnLerp.Reset();

            PhononCore.iplDestroyBinauralEffect(ref directBinauralEffect);
            directBinauralEffect = IntPtr.Zero;

            PhononCore.iplDestroyPanningEffect(ref directCustomPanningEffect);
            directCustomPanningEffect = IntPtr.Zero;

        }

        public void AudioFrameUpdate(float[] data, int channels, bool physicsBasedAttenuation, float directMixFraction, 
            bool directBinauralEnabled, HRTFInterpolation hrtfInterpolation)
        {
            float distanceAttenuation = (physicsBasedAttenuation) ? directSoundPath.distanceAttenuation : 1f;
            directAttnLerp.Set(directSoundPath.occlusionFactor * directMixFraction * distanceAttenuation);

            float perSampleIncrement;
            int numFrames = data.Length / channels;
            float attnFactor = directAttnLerp.Update(out perSampleIncrement, numFrames);
            Vector3 directDirection = directSoundPath.direction;

            AudioBuffer inputBuffer;
            inputBuffer.audioFormat = inputFormat;
            inputBuffer.numSamples = data.Length / channels;
            inputBuffer.deInterleavedBuffer = null;
            inputBuffer.interleavedBuffer = data;

            AudioBuffer outputBuffer;
            outputBuffer.audioFormat = outputFormat;
            outputBuffer.numSamples = data.Length / channels;
            outputBuffer.deInterleavedBuffer = null;
            outputBuffer.interleavedBuffer = data;

            if ((outputFormat.channelLayout == ChannelLayout.Stereo) && directBinauralEnabled)
            {
                // Apply binaural audio to direct sound.
                PhononCore.iplApplyBinauralEffect(directBinauralEffect, inputBuffer, directDirection, hrtfInterpolation, 
                    outputBuffer);
            }
            else if (outputFormat.channelLayout == ChannelLayout.Custom)
            {
                // Apply panning fo custom speaker layout.
                PhononCore.iplApplyPanningEffect(directCustomPanningEffect, inputBuffer, directDirection, outputBuffer);
            }

            // Process direct sound occlusion
            for (int i = 0, count = 0; i < numFrames; ++i)
            {
                for (int j = 0; j < channels; ++j, ++count)
                    data[count] *= attnFactor;

                attnFactor += perSampleIncrement;
            }
        }

        public void FrameUpdate(IntPtr envRenderer, Vector3 sourcePosition, Vector3 listenerPosition, 
            Vector3 listenerAhead, Vector3 listenerUp, float partialOcclusionRadius, OcclusionOption directOcclusionOption)
        {
            directSoundPath = PhononCore.iplGetDirectSoundPath(envRenderer, listenerPosition, listenerAhead, listenerUp, 
                sourcePosition, partialOcclusionRadius, directOcclusionOption);
        }

        AudioFormat inputFormat;
        AudioFormat outputFormat;
        DirectSoundPath directSoundPath;

        // Per sample interpolator.
        AttenuationInterpolator directAttnLerp = new AttenuationInterpolator();
        int directAttnLerpFrames = 4;

        // Phonon API related variables.
        IntPtr binauralRenderer = IntPtr.Zero;
        IntPtr directBinauralEffect = IntPtr.Zero;
        IntPtr directCustomPanningEffect = IntPtr.Zero;
    }
}