function E(className, props, children){
    var el = document.createElement("div");
    el.className = className;
    for(var prop in props){
        el[prop] = props[prop];
    }

    for(var i = 0; i < children.length; i++){
        var child = children[i];
        if(typeof child === "object"){
            el.appendChild(child);
        } else {
            el.appendChild(document.createTextNode(child));
        }  
    }

    return el;
}