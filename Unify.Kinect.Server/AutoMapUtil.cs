using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unify.Kinect.Messages;

namespace Unify.Kinect.Server
{
  public class AutoMapUtil
  {
    public static void Initialise()
    {
      AutoMapper.Mapper.CreateMap<BoneOrientation, BoneOrientationMessage>();
      AutoMapper.Mapper.CreateMap<BoneRotation, BoneRotationMessage>();
      AutoMapper.Mapper.CreateMap<FrameEdges, FrameEdgesMessage>();

      AutoMapper.Mapper.CreateMap<Joint, JointMessage>();
      AutoMapper.Mapper.CreateMap<JointTrackingState, JointTrackingStateMessage>();
      AutoMapper.Mapper.CreateMap<JointType, JointTypeMessage>();

      AutoMapper.Mapper.CreateMap<Matrix4, Matrix4Message>();
      AutoMapper.Mapper.CreateMap<Vector4, Vector4Message>();
      AutoMapper.Mapper.CreateMap<Skeleton, SkeletonMessage>();

      AutoMapper.Mapper.CreateMap<SkeletonPoint, SkeletonPointMessage>();
      AutoMapper.Mapper.CreateMap<SkeletonTrackingState, SkeletonTrackingStateMessage>();
      
    }
  }
}
