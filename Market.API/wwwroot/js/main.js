// Market API - Main JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Set active navigation link based on current page
    setActiveNavLink();

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

// Set active navigation link
function setActiveNavLink() {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-links a');
    
    navLinks.forEach(link => {
        const linkPath = new URL(link.href).pathname;
        
        // Exact match or home page
        if (linkPath === currentPath || 
            (currentPath === '/' && linkPath === '/') ||
            (currentPath === '/index.html' && linkPath === '/')) {
            link.classList.add('active');
        } else {
            link.classList.remove('active');
        }
    });
}

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
    const pre = block.parentElement;
    
    // Create copy button
    const copyButton = document.createElement('button');
    copyButton.className = 'copy-button';
    copyButton.textContent = 'Copy';
    copyButton.setAttribute('aria-label', 'Copy code to clipboard');
    
    // Insert button
    pre.style.position = 'relative';
    pre.appendChild(copyButton);
    
    copyButton.addEventListener('click', async function() {
        const text = block.textContent;
        
        try {
            await navigator.clipboard.writeText(text);
            
            // Show success state
            copyButton.textContent = 'Copied!';
            copyButton.classList.add('copied');
            
            // Reset after 2 seconds
            setTimeout(() => {
                copyButton.textContent = 'Copy';
                copyButton.classList.remove('copied');
            }, 2000);
        } catch (err) {
            console.error('Failed to copy:', err);
            copyButton.textContent = 'Failed';
            setTimeout(() => {
                copyButton.textContent = 'Copy';
            }, 2000);
        }
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
