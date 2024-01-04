# Stage 1: Build the .NET 8 backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend
WORKDIR /app
COPY ["source/BudgetManager.Api/BudgetManager.Api.csproj", "source/BudgetManager.Api/"]
COPY ["source/BudgetManager.Application/BudgetManager.Application.csproj", "source/BudgetManager.Application/"]
COPY ["source/BudgetManager.Domain/BudgetManager.Domain.csproj", "source/BudgetManager.Domain/"]
COPY ["source/BudgetManager.Infrastructure/BudgetManager.Infrastructure.csproj", "source/BudgetManager.Infrastructure/"]
RUN dotnet restore source/BudgetManager.Api/BudgetManager.Api.csproj
COPY . .
RUN dotnet publish source/BudgetManager.Api/BudgetManager.Api.csproj -c Release -o /publish

# Stage 2: Build the Vue 3 frontend
FROM node:14 AS build-frontend
WORKDIR /app/source/BudgetManager.UI
ARG AAD_REDIRECT_URL
ARG AAD_TENANT_ID
ARG AAD_CLIENT_ID
ARG API_URL
ARG ENVIRONMENT
COPY ./source/BudgetManager.UI/package*.json ./
RUN npm install
COPY ./source/BudgetManager.UI/. .
RUN echo "VUE_APP_AAD_REDIRECT=${AAD_REDIRECT_URL}" > .env.production
RUN echo "VUE_APP_AAD_TENANT=${AAD_TENANT_ID}" >> .env.production
RUN echo "VUE_APP_AAD_CLIENT_ID=${AAD_CLIENT_ID}" >> .env.production
RUN echo "VUE_APP_API_URL=${API_URL}" >> .env.production
RUN echo "VUE_APP_ENV=${ENVIRONMENT}" >> .env.production
RUN npm run build -- -configuration --production --env-file .env.production

# Stage 3: Set up Nginx for frontend and combine with backend
FROM nginx:latest AS final
COPY --from=build-frontend /app/source/BudgetManager.UI/dist/ /usr/share/nginx/html/
COPY --from=build-backend /publish /app/api/
COPY ./nginx-custom.conf /etc/nginx/conf.d/
EXPOSE 80

# Environment variables for both frontend and backend
ENV API_BASE_URL=http://localhost/api/
ENV AAD_REDIRECT_URL=default_redirect_url
ENV AAD_TENANT_ID=default_tenant_id
ENV AAD_CLIENT_ID=default_client_id
ENV API_URL=default_api_url
ENV ENVIRONMENT=development

CMD ["nginx", "-g", "daemon off;"]
