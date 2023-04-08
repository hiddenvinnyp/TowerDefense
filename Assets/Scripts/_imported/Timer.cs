public class Timer
{
    private float m_CurrentTime;
    private float m_LoopTime;
    private bool m_IsLooped = false;
    public bool IsFinished => m_CurrentTime <= 0;

    public Timer(float startTime)
    {
        m_LoopTime = startTime;
        Start(startTime);
    } 

    public Timer(float startTime, bool isLooped)
    {
        m_IsLooped = isLooped;
        m_LoopTime = startTime;
        Start(startTime);
    }

    public void Start (float startTime)
    {
        m_CurrentTime = startTime;
    }

    public void RemoveTime (float deltaTime)
    {
        if (m_CurrentTime <= 0 && !m_IsLooped) return;
        if (m_CurrentTime <= 0 && m_IsLooped)
        {
            m_CurrentTime = m_LoopTime;
        }

        m_CurrentTime -= deltaTime;
    }

    public void Restart()
    {
        Start(m_LoopTime);
    }
}
