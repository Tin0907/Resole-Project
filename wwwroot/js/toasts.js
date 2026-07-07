// Centralized toast UI for the site
(function () {
    function createContainer() {
        let c = document.getElementById('toastContainer');
        if (c) return c;
        c = document.createElement('div');
        c.id = 'toastContainer';
        c.style.position = 'fixed';
        c.style.top = '24px';
        c.style.right = '24px';
        c.style.zIndex = 99999;
        c.style.display = 'flex';
        c.style.flexDirection = 'column';
        c.style.gap = '12px';
        document.body.appendChild(c);
        return c;
    }

    function showToast(type, title, message, duration) {
        duration = duration || 3500;
        const container = createContainer();
        const el = document.createElement('div');
        el.className = 'site-toast ' + (type || 'info');
        el.style.minWidth = '300px';
        el.style.maxWidth = '420px';
        el.style.padding = '12px 16px';
        el.style.borderRadius = '8px';
        el.style.boxShadow = '0 8px 30px rgba(0,0,0,0.2)';
        el.style.color = '#fff';
        el.style.display = 'flex';
        el.style.gap = '12px';
        el.style.alignItems = 'flex-start';
        el.style.transform = 'translateX(120%)';
        el.style.opacity = '0';
        el.style.transition = 'all 0.35s ease';

        const body = document.createElement('div');
        body.style.flex = '1';
        const t = document.createElement('div');
        t.style.fontWeight = '700';
        t.style.marginBottom = '6px';
        t.style.fontSize = '0.85rem';
        t.textContent = title || '';
        const m = document.createElement('div');
        m.style.fontWeight = '400';
        m.style.fontSize = '0.95rem';
        m.textContent = message || '';
        body.appendChild(t);
        body.appendChild(m);

        const btn = document.createElement('button');
        btn.className = 'btn btn-sm btn-light';
        btn.style.marginLeft = '8px';
        btn.textContent = 'OK';
        btn.onclick = () => el.remove();

        el.appendChild(body);
        el.appendChild(btn);

        // color by type
        if (type === 'success') el.style.background = 'linear-gradient(135deg,#10b981,#059669)';
        else if (type === 'error') el.style.background = 'linear-gradient(135deg,#ef4444,#dc2626)';
        else if (type === 'warning') el.style.background = 'linear-gradient(135deg,#f59e0b,#d97706)';
        else el.style.background = 'linear-gradient(135deg,#667eea,#764ba2)';

        container.appendChild(el);
        requestAnimationFrame(() => {
            el.style.transform = 'translateX(0)';
            el.style.opacity = '1';
        });

        setTimeout(() => {
            el.style.transform = 'translateX(120%)';
            el.style.opacity = '0';
            setTimeout(() => el.remove(), 400);
        }, duration);
    }

    // expose
    window.showToast = showToast;
})();
