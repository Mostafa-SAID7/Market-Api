// Market API - Main JavaScript

document.addEventListener('DOMContentLoaded', function() {
    initLucideIcons();
    setActiveNavLink();
    updateCopyrightYear();
    initSmoothScrolling();
    initCardObserver();
    initCodeCopyButtons();
    checkApiStatus();
});

// Dynamic copyright year in footers
function updateCopyrightYear() {
    const years = document.querySelectorAll('.copyright-year');
    const currentYear = new Date().getFullYear();
    years.forEach(el => {
        el.textContent = currentYear;
    });
}

// Initialize Lucide icons
function initLucideIcons() {
    if (typeof lucide !== 'undefined' && typeof lucide.createIcons === 'function') {
        lucide.createIcons();
    }
}

// Set active navigation link based on current path
function setActiveNavLink() {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-links a');
    
    navLinks.forEach(link => {
        const linkPath = new URL(link.href, window.location.origin).pathname;
        
        if (linkPath === currentPath || 
            (currentPath === '/' && linkPath === '/index.html') ||
            (currentPath === '/index.html' && linkPath === '/index.html')) {
            link.classList.add('active');
            link.setAttribute('aria-current', 'page');
        } else {
            link.classList.remove('active');
            link.removeAttribute('aria-current');
        }
    });
}

// Smooth scrolling for anchor links
function initSmoothScrolling() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const href = this.getAttribute('href');
            if (href === '#') return;
            const target = document.querySelector(href);
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Intersection Observer for Card reveal animations (respects prefers-reduced-motion)
function initCardObserver() {
    const cards = document.querySelectorAll('.card');
    if (!cards.length) return;

    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    if (prefersReducedMotion || !('IntersectionObserver' in window)) {
        cards.forEach(card => card.classList.add('is-visible'));
        return;
    }

    const observer = new IntersectionObserver((entries, obs) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('is-visible');
                obs.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.1,
        rootMargin: '0px 0px -40px 0px'
    });

    cards.forEach(card => observer.observe(card));
}

// Check API status with timeout and class toggling
async function checkApiStatus() {
    const statusElement = document.querySelector('.status');
    if (!statusElement) return;

    const statusText = statusElement.querySelector('span:last-child');
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), 5000);

    try {
        const response = await fetch('/api/products', {
            signal: controller.signal
        });
        clearTimeout(timeoutId);

        if (response.ok) {
            if (statusText) statusText.textContent = 'API Running';
            statusElement.classList.add('status--running');
            statusElement.classList.remove('status--offline');
        } else {
            throw new Error(`HTTP ${response.status}`);
        }
    } catch (error) {
        clearTimeout(timeoutId);
        if (statusText) statusText.textContent = 'API Offline';
        statusElement.classList.add('status--offline');
        statusElement.classList.remove('status--running');
    }
}

// Code block copy button implementation
function initCodeCopyButtons() {
    document.querySelectorAll('pre code').forEach(block => {
        const pre = block.parentElement;
        if (!pre || pre.querySelector('.copy-button')) return;

        const copyButton = document.createElement('button');
        copyButton.type = 'button';
        copyButton.className = 'copy-button';
        copyButton.textContent = 'Copy';
        copyButton.setAttribute('aria-label', 'Copy code to clipboard');

        pre.appendChild(copyButton);

        copyButton.addEventListener('click', async function() {
            const text = block.textContent || '';

            try {
                if (navigator.clipboard && navigator.clipboard.writeText) {
                    await navigator.clipboard.writeText(text);
                } else {
                    fallbackCopyText(text);
                }
                
                copyButton.textContent = 'Copied!';
                copyButton.classList.add('copied');

                setTimeout(() => {
                    copyButton.textContent = 'Copy';
                    copyButton.classList.remove('copied');
                }, 2000);
            } catch (err) {
                copyButton.textContent = 'Failed';
                setTimeout(() => {
                    copyButton.textContent = 'Copy';
                }, 2000);
            }
        });
    });
}

// Fallback clipboard method for restricted environments
function fallbackCopyText(text) {
    const textArea = document.createElement('textarea');
    textArea.value = text;
    textArea.style.position = 'fixed';
    textArea.style.opacity = '0';
    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();
    try {
        document.execCommand('copy');
    } finally {
        document.body.removeChild(textArea);
    }
}

// Explorer Page Workbench Interactive Logic
document.addEventListener('DOMContentLoaded', function() {
    initExplorerPage();
    initStatusDashboard();
});

function initExplorerPage() {
    const sendBtn = document.getElementById('explorer-send');
    if (!sendBtn) return;

    const methodSelect = document.getElementById('explorer-method');
    const urlInput = document.getElementById('explorer-url');
    const bodyInput = document.getElementById('explorer-body');
    const outputEl = document.getElementById('response-output');
    const statusText = document.getElementById('response-status-text');
    const statusBadge = document.getElementById('response-status-badge');
    const timeEl = document.getElementById('response-time');

    const samplePayloads = {
        products: '{\n  "name": "Wireless Mechanical Keyboard",\n  "description": "RGB Backlit, Hot-Swappable",\n  "price": 129.99,\n  "quantity": 50,\n  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",\n  "vendorId": "4ba85f64-5717-4562-b3fc-2c963f66afa6"\n}',
        categories: '{\n  "name": "Electronics",\n  "slug": "electronics"\n}',
        users: '{\n  "firstName": "John",\n  "lastName": "Doe",\n  "email": "john.doe@example.com",\n  "role": 0\n}',
        vendors: '{\n  "storeName": "Apex Tech Store",\n  "userId": "5ca85f64-5717-4562-b3fc-2c963f66afa6"\n}',
        orders: '{\n  "userId": "5ca85f64-5717-4562-b3fc-2c963f66afa6",\n  "items": []\n}',
        carts: '{\n  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",\n  "quantity": 1\n}',
        reviews: '{\n  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",\n  "rating": 5,\n  "comment": "Outstanding build quality!"\n}'
    };

    // Handle Preset Buttons
    document.querySelectorAll('.preset-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            document.querySelectorAll('.preset-btn').forEach(b => b.classList.remove('btn-primary'));
            this.classList.add('btn-primary');

            const entity = this.getAttribute('data-entity');
            const method = this.getAttribute('data-method');
            const path = this.getAttribute('data-path');

            if (methodSelect) methodSelect.value = method;
            if (urlInput) urlInput.value = path;
            if (bodyInput && samplePayloads[entity]) {
                bodyInput.value = samplePayloads[entity];
            }
        });
    });

    // Send Request Execution
    sendBtn.addEventListener('click', async function() {
        const url = urlInput.value;
        const method = methodSelect.value;
        const bodyText = bodyInput.value.trim();

        outputEl.textContent = 'Sending request...';
        const startTime = performance.now();

        try {
            const options = { method, headers: {} };
            if ((method === 'POST' || method === 'PUT') && bodyText) {
                options.headers['Content-Type'] = 'application/json';
                options.body = bodyText;
            }

            const res = await fetch(url, options);
            const duration = Math.round(performance.now() - startTime);

            if (timeEl) timeEl.textContent = `${duration} ms`;
            if (statusText) statusText.textContent = `${res.status} ${res.statusText}`;

            if (statusBadge) {
                statusBadge.className = res.ok ? 'status status--running' : 'status status--offline';
            }

            const contentType = res.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                const json = await res.json();
                outputEl.textContent = JSON.stringify(json, null, 2);
            } else {
                const text = await res.text();
                outputEl.textContent = text || `// HTTP ${res.status} ${res.statusText}`;
            }
        } catch (err) {
            const duration = Math.round(performance.now() - startTime);
            if (timeEl) timeEl.textContent = `${duration} ms`;
            if (statusText) statusText.textContent = 'Network Error / API Offline';
            if (statusBadge) statusBadge.className = 'status status--offline';
            outputEl.textContent = `Error connecting to ${url}.\nEnsure Market.API is running on localhost.`;
        }
    });
}

function initStatusDashboard() {
    const refreshBtn = document.getElementById('refresh-status-btn');
    if (!refreshBtn) return;

    const latencyText = document.getElementById('api-latency-text');
    const lastChecked = document.getElementById('status-last-checked');

    refreshBtn.addEventListener('click', async function() {
        const startTime = performance.now();
        try {
            const res = await fetch('/api/products');
            const duration = Math.round(performance.now() - startTime);
            if (latencyText) latencyText.textContent = `${duration} ms`;
            if (lastChecked) lastChecked.textContent = new Date().toLocaleTimeString();
        } catch (err) {
            if (latencyText) latencyText.textContent = 'Offline';
            if (lastChecked) lastChecked.textContent = new Date().toLocaleTimeString();
        }
    });
}
