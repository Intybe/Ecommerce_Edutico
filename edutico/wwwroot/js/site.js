document.addEventListener('DOMContentLoaded', function () {
    const inputFile = document.querySelector('#picture_input');
    const pictureImage = document.querySelector('.picture__image'); // Deve ser um <span>
    const picturePlaceholder = document.querySelector('.picture__placeholder');
    const pictureImageTxt = "Escolha uma imagem";

    // Verifica se o elemento pictureImage existe
    if (pictureImage && picturePlaceholder) {
        // Inicialmente, mantém o placeholder visível
        picturePlaceholder.style.display = 'block';
        pictureImage.innerHTML = ''; // Não precisa de texto aqui, pois a imagem virá depois

        inputFile.addEventListener('change', function (e) {
            const inputTarget = e.target;
            const file = inputTarget.files[0];

            if (file) {
                const reader = new FileReader();

                reader.addEventListener('load', function (e) {
                    const readerTarget = e.target;

                    const img = document.createElement('img');
                    img.src = readerTarget.result;
                    img.classList.add('picture__img');

                    // Limpa o placeholder e insere a imagem carregada
                    picturePlaceholder.style.display = 'none'; // Oculta o placeholder
                    pictureImage.innerHTML = ''; // Limpa o conteúdo
                    pictureImage.appendChild(img); // Adiciona a nova imagem
                });

                reader.readAsDataURL(file);
            } else {
                // Mostra o placeholder novamente se o arquivo for removido ou inválido
                picturePlaceholder.style.display = 'block';
                pictureImage.innerHTML = ''; // Limpa qualquer imagem que possa estar presente
            }
        });
    } else {
        console.error("Elemento .picture__image ou .picture__placeholder não foi encontrado.");
    }
});

// Extender filtros nas telas de Busca
document.querySelectorAll('.filter-title').forEach(item => {
    item.addEventListener('click', () => {
        const content = item.nextElementSibling;
        content.style.display = content.style.display === 'block' ? 'none' : 'block';
        item.classList.toggle('active');
    });
});

//Ver mais em detalhes do produto
document.getElementById("toggle-button").addEventListener("click", function () {
    var moreContent = document.getElementById("versaocompleta");
    var button = document.getElementById("toggle-button");

    if (moreContent.classList.contains("hidden")) {
        moreContent.classList.remove("hidden");
        button.textContent = "Mostrar menos";
    } else {
        moreContent.classList.add("hidden");
        button.textContent = "Ler descrição completa";
    }

});

// Quantidade de avaliações
const totalRatings = 10;

// Distribuição das avaliações
const ratingsData = {
    5: 8,
    4: 1,
    3: 0,
    2: 0,
    1: 0
};

// Atualiza a largura das barras de progresso
for (let rating in ratingsData) {
    const percentage = (ratingsData[rating] / totalRatings) * 100;
    document.getElementById(`bar-${rating}`).style.width = `${percentage}%`;
}

// Exibe o total de avaliações
document.getElementById('total-ratings').innerText = totalRatings;

