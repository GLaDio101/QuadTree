using Project.GameEntity;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Project.QueadTree
{
    /// <summary>
    /// Queries a QuadTree to test for collisions with only nearby bodies
    /// </summary>
    public class CollisionSystemQuadTree : CollisionSystem
    {
        ///// Constructor /////

        public CollisionSystemQuadTree(QuadTree tree)
        {
            QuadTree = tree;
        }

        ///// Fields /////

        public QuadTree QuadTree { get; set; }
        public EntityQueryBuilder Entities { get; set; }

        ///// Methods /////


        public override void DetectBodyVsBody()
        {
            Entities.ForEach((Entity entity, ref Translation translation) =>
            {
                var bodyComponent = EntityManager.GetSharedComponentData<BodyComponent>(entity);

                if (!bodyComponent.Sleeping)
                {
                    // todo: something better maybe?
                    var maxDist = bodyComponent.CollisionShape.Extents.x;
                    maxDist = Mathf.Max(maxDist, bodyComponent.CollisionShape.Extents.y);
                    maxDist = Mathf.Max(maxDist, bodyComponent.CollisionShape.Extents.z);

                    var ents = QuadTree.GetBodies(bodyComponent.CollisionShape.Center, maxDist);
                    for (int j = 0; j < ents.Count; j++)
                    {
                        var body2 = ents[j];
                        var bodyComponent2 = EntityManager.GetSharedComponentData<BodyComponent>(body2);

                        if (body2 == Entity.Null
                            ||
                            bodyComponent2.Sleeping
                            || Equals(bodyComponent.GetHashCode(), bodyComponent2.GetHashCode()))
                        {
                            continue;
                        }

                        Test(entity, body2);
                    }
                }
            });
        }

        public override bool LineOfSight(Vector3 start, Vector3 end)
        {
            for (var i = 0; i < bodyList.Count; i++)
            {
                var bodyComponent = EntityManager.GetSharedComponentData<BodyComponent>(bodyList[i]);

                if (CollisionTest.SegmentIntersects(bodyComponent.CollisionShape, start, end))
                    return false;
            }

            return true;
        }
    }
}