// Market API - Main JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Add animation on scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    // Observe all cards
    document.querySelectorAll('.card').forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });

    // Check API status
    checkApiStatus();
});

// Check if API is running
async function checkApiStatus() {
    const statusElement = document.querySelector('.status');
    if (!statusElement) return;

    try {
        const response = await fetch('/api/products');
        if (response.ok) {
            statusElement.querySelector('span:last-child').textContent = 'API Running';
            statusElement.style.background = 'rgba(76, 175, 80, 0.2)';
            statusElement.style.borderColor = '#4caf50';
        }
    } catch (error) {
        statusElement.querySelector('span:last-child').textContent = 'API Offline';
        statusElement.style.background = 'rgba(244, 67, 54, 0.2)';
        statusElement.style.borderColor = '#f44336';
        statusElement.querySelector('.status-dot').style.background = '#f44336';
    }
}

// Add parallax effect to hero section
window.addEventListener('scroll', function() {
    const hero = document.querySelector('.hero');
    if (hero) {
        const scrolled = window.pageYOffset;
        hero.style.transform = `translateY(${scrolled * 0.5}px)`;
        hero.style.opacity = 1 - (scrolled / 500);
    }
});

// Copy code to clipboard functionality
document.querySelectorAll('pre code').forEach(block => {
    block.style.position = 'relative';
    block.style.cursor = 'pointer';
    
    block.addEventListener('click', function() {
        const text = this.textContent;
        navigator.clipboard.writeText(text).then(() => {
            // Show copied notification
            const notification = document.createElement('div');
            notification.textContent = 'Copied!';
            notification.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: var(--accent-gold);
                color: var(--dark-bg);
                padding: 1rem 2rem;
                border-radius: 8px;
                font-weight: 600;
                z-index: 10000;
                animation: slideIn 0.3s ease;
            `;
            document.body.appendChild(notification);
            
            setTimeout(() => {
                notification.style.animation = 'slideOut 0.3s ease';
                setTimeout(() => notification.remove(), 300);
            }, 2000);
        });
    });
});

// Add CSS animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);
