document.addEventListener('DOMContentLoaded', function () {
    // Verifica se os elementos de upload de imagem estão disponíveis
    const inputFile = document.querySelector('#picture_input');
    const pictureImage = document.querySelector('.picture__image'); // Deve ser um <span>
    const picturePlaceholder = document.querySelector('.picture__placeholder');

    /*if (pictureImage && picturePlaceholder) {
        picturePlaceholder.style.display = 'block';
        pictureImage.innerHTML = ''; // Limpa qualquer conteúdo inicial

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

                    picturePlaceholder.style.display = 'none';
                    pictureImage.innerHTML = '';
                    pictureImage.appendChild(img);
                });

                reader.readAsDataURL(file);
            } else {
                picturePlaceholder.style.display = 'block';
                pictureImage.innerHTML = '';
            }
        });
    } else {
        console.error("Elemento .picture__image ou .picture__placeholder não foi encontrado.");
    }*/

    // Ver mais em detalhes do produto
    const toggleButton = document.getElementById("toggle-button");
    const moreContent = document.getElementById("versaocompleta");

    if (toggleButton && moreContent) {
        toggleButton.addEventListener("click", function () {
            if (moreContent.classList.contains("hidden")) {
                moreContent.classList.remove("hidden");
                toggleButton.textContent = "Mostrar menos";
            } else {
                moreContent.classList.add("hidden");
                toggleButton.textContent = "Ler descrição completa";
            }
        });
    } else {
        console.error("Elemento #toggle-button ou #versaocompleta não foi encontrado.");
    }

    

    // Extender filtros nas telas de Busca
    document.querySelectorAll('.filter-title').forEach(item => {
        item.addEventListener('click', () => {
            const content = item.nextElementSibling;
            content.style.display = content.style.display === 'block' ? 'none' : 'block';
            item.classList.toggle('active');
        });
    });

    // função para editar campos da tela Meu Perfil
    function edicao(botao, ids) {
        //loop para enqunto tiver campos no array
        ids.forEach(function (id) {
            var inputField = document.getElementById(id);

            if (inputField.hasAttribute('disabled')) {
                inputField.removeAttribute('disabled');
                inputField.focus();
                botao.textContent = 'Salvar';
            } else {
                inputField.setAttribute('disabled', true);
                botao.textContent = 'Modificar';
            }
        });
    }
    /*Trocar imagem na tela Detalhes produto*/
    // Seleciona todas as imagens pequenas
    const imgsPequenas = document.querySelectorAll('.imgpequena');
    // Seleciona a imagem principal
    const imgPrincipal = document.querySelector('.imgprincipal');

    // Adiciona um evento de clique a cada imagem pequena
    imgsPequenas.forEach(img => {
        img.addEventListener('click', function () {
            // Atualiza o src da imgprincipal para o src da imagem pequena clicada
            imgPrincipal.src = this.src;
        });
    });

    function selecionarHabilidade(chip) {
        // Adiciona ou remove a classe 'selected' ao chip
        chip.classList.toggle('selected');

        // Obtém todas as habilidades selecionadas
        const habilidadesSelecionadas = document.getElementById('habilidadesSelecionadas');
        const selectedChips = document.querySelectorAll('.chip.selected');

        // Cria um array com as habilidades selecionadas
        const habilidades = Array.from(selectedChips).map(chip => chip.textContent);

        // Atualiza o campo escondido com as habilidades selecionadas
        habilidadesSelecionadas.value = habilidades.join(', ');
    }

    /*Quantidade de carrinho */
    const valorInput = document.getElementById("valor");
    const incrementarBtn = document.getElementById("incrementar");
    const decrementarBtn = document.getElementById("decrementar");

    incrementarBtn.addEventListener("click", function () {
        valorInput.value = parseInt(valorInput.value) + 1;
    });

    decrementarBtn.addEventListener("click", function () {
        if (valorInput.value > 0) {  // Evitar valores negativos
            valorInput.value = parseInt(valorInput.value) - 1;
        }
    });

    valorInput.addEventListener("input", function () {
        // Evitar que o input tenha valores negativos
        if (valorInput.value < 0) {
            valorInput.value = 0;
        }
    });

    // Função para fechar o card
    const fecharCard = document.getElementById("fecharCard");
    const cardProduto = document.getElementById("cardProduto");

    fecharCard.addEventListener("click", function () {
        cardProduto.style.display = "none";
    });
  



});
