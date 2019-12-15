using System.Collections.Generic;
using Project.GameEntity;
using Project.Manager;
using Project.QueadTree.Shapes;
using Unity.Entities;
using UnityEngine;

namespace Project.QueadTree
{
    public abstract class CollisionSystem
    {
        ///// Fields /////

        protected List<Entity> bodyList = new List<Entity>(MaxCollisionBodies);
        private HashSet<int> _pairs = new HashSet<int>();
        private List<int> _pairCache = new List<int>();
        private int _uniqueIndex;
        public EntityManager EntityManager;

        public const int MaxCollisionBodies = 10000;

        ///// Methods /////

        public abstract void DetectBodyVsBody();
        public abstract bool LineOfSight(Vector3 start, Vector3 end);

        /// <summary>
        /// Adds a body to the CollisionSystem
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual bool AddBody(Entity body)
        {
            if (!bodyList.Contains(body) && bodyList.Count < MaxCollisionBodies)
            {
                var sharedComponentData = EntityManager.GetSharedComponentData<BodyComponent>(body);
                sharedComponentData.RefId = _uniqueIndex;
                _uniqueIndex++;
                bodyList.Add(body);
                return true;
            }

            return false;
        }

        /// <returns></returns>
        public virtual void Clear()
        {
            bodyList.Clear();
        }


        /// <summary>
        /// Process CollisionSystem by one step
        /// </summary>
        public virtual void Step()
        {
            DetectBodyVsBody();


            // This was implemented for CollisionSystem implementations with broad phases
            // When two colliders are paired and one of them is moved to a far away position 
            // on the same frame, they wont be tested next frame due to broad phasing, but they will still be paired.
            // This simply checks all pairs that weren't checked this frame

            foreach (var i in _pairs)
            {
                var body1 = FindCollisionBody(i / (MaxCollisionBodies + 1));
                var body2 = FindCollisionBody(i % (MaxCollisionBodies + 1));
                if (body1 == Entity.Null || body2 == Entity.Null)
                {
                    continue;
                }

                Test(body1, body2, false);
            }

            _pairs.Clear();

            for (int i = 0; i < _pairCache.Count; i++)
            {
                _pairs.Add(_pairCache[i]);
            }

            _pairCache.Clear();
        }

        public Entity FindCollisionBody(int refId)
        {
            for (int i = 0; i < bodyList.Count; i++)
            {
                var sharedComponentData = EntityManager.GetSharedComponentData<BodyComponent>(bodyList[i]);

                if (sharedComponentData.RefId == refId)
                    return bodyList[i];
            }

            return Entity.Null;
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.black;
            for (var i = 0; i < bodyList.Count; i++)
            {
                var sharedComponentData = EntityManager.GetSharedComponentData<BodyComponent>(bodyList[i]);

                var center = sharedComponentData.CollisionShape.Center;
                if (center == Vector3.zero) continue;
                center.y += 2f;
                Gizmos.DrawWireCube(center, sharedComponentData.CollisionShape.Extents * 2);
            }
        }

        public virtual bool RemoveBody(Entity entity)
        {
            var bodyComponent = EntityManager.GetSharedComponentData<BodyComponent>(entity);
            var body = (ICollisionBody) bodyComponent;
            _pairs.Remove(body.RefId);
            return bodyList.Remove(entity);
        }

        /// <summary>
        ///  Executes collision between two bodies
        /// </summary>
        /// <param name="body1"></param>
        /// <param name="entity2"></param>
        /// <param name="removePair"></param>
        /// <returns></returns>
        protected bool Test(Entity entity1, Entity entity2, bool removePair = true)
        {
            var body1 = EntityManager.GetSharedComponentData<BodyComponent>(entity1);
            var body2 = EntityManager.GetSharedComponentData<BodyComponent>(entity2);

            var result = new CollisionResult();
            var paired = FindCollisionPair(body1, body2, removePair);

            if (TestCollisionShapes(body1.CollisionShape, body2.CollisionShape, ref result))
            {
                result.Type = paired ? CollisionType.Stay : CollisionType.Enter;
                if (!paired)
                {
                    if (!body1.StayMap.ContainsKey(body2.GetHashCode()))
                    {
                        var lifeComponent1 = EntityManager.GetComponentData<LifeComponent>(entity1);
                        lifeComponent1.Life--;
                        EntityManager.SetComponentData(entity1, lifeComponent1);
                        EntityManager.SetComponentData(entity1, new ColorEffectData()
                        {
                            Duration = .5f,
                            EffectType = ColorEffectEnum.Damage.GetHashCode()
                        });
                        if (lifeComponent1.Life == 0)
                            RemoveBody(entity1);
                    }

                    if (!body2.StayMap.ContainsKey(body1.GetHashCode()))
                    {
                        var lifeComponent2 = EntityManager.GetComponentData<LifeComponent>(entity2);
                        lifeComponent2.Life--;
                        EntityManager.SetComponentData(entity2, lifeComponent2);
                        EntityManager.SetComponentData(entity2, new ColorEffectData()
                        {
                            Duration = .5f,
                            EffectType = ColorEffectEnum.Damage.GetHashCode()
                        });
                        if (lifeComponent2.Life == 0)
                            RemoveBody(entity2);
                    }
                }

                CacheCollisionPair(body1, body2);
                body2.OnCollision(result, body1);
                result.Normal *= -1;
                result.First = true;
                body1.OnCollision(result, body2);
                return true;
            }
            else
            {
                if (paired)
                {
                    result.Type = CollisionType.Exit;
                    body2.OnCollision(result, body1);
                    result.Normal *= -1;
                    result.First = true;
                    body1.OnCollision(result, body2);
                    return true;
                }
            }

            return false;
        }

        private bool FindCollisionPair(ICollisionBody a, ICollisionBody b, bool remove = true)
        {
            var idx = a.RefId * (MaxCollisionBodies + 1) + b.RefId;
            if (remove) return _pairs.Remove(idx);
            return _pairs.Contains(idx);
        }

        private void CacheCollisionPair(ICollisionBody a, ICollisionBody b)
        {
            var idx = a.RefId * (MaxCollisionBodies + 1) + b.RefId;
            _pairCache.Add(idx);
        }

        private static bool TestCollisionShapes(ICollisionShape a, ICollisionShape b, ref CollisionResult result)
        {
            result = ((BoxShape) a).TestCollision(b);
            return result.Collides;
        }
    }
}