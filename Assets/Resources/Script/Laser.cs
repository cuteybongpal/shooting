using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void OnParticleCollision(GameObject other)
    {
        int count = particleSystem.GetCollisionEvents(other, collisionEvents);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int num = particleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 hitPos = collisionEvents[i].intersection;

            // 충돌 지점 주변 입자만 제거
            for (int j = 0; j < num; j++)
            {
                Vector3 particleWorldPos = particleSystem.transform.TransformPoint(particles[j].position);
                if (Vector3.Distance(particleWorldPos, hitPos) < 1f)
                {
                    particles[j].remainingLifetime = 0f;
                    EnemyController enemyController = other.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.damaged(1);
                    }
                    else
                    {
                        BossController boss = other.GetComponent<BossController>();
                        if (boss == null)
                            return;
                        boss.damaged(1);
                    }
                    
                }
            }
        }

        particleSystem.SetParticles(particles, num);
    }
}
